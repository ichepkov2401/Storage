using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Reflection;

namespace Storage.Data.Contexts
{
    /// <summary>
    /// Провайдер извлечения данных из JSON файла (Id только целочисленного типа)
    /// </summary>
    internal abstract class FileContext
    {
        private Dictionary<string, int> indexes = new Dictionary<string, int>();
        private string path;
        private List<PropertyInfo> sets = new List<PropertyInfo>();
        private List<IList> oneToMany;
        private JsonResolver jsonResolver = new JsonResolver();

        /// <summary>
        /// Настройка подключения - извлечение данных из JSON хранилища
        /// </summary>
        /// <param name="path">Путь к файлу данных</param>
        internal FileContext(string path)
        {
            this.path = path;
            using (StreamReader reader = new StreamReader(this.path))
            {
                JObject data = JObject.Parse(reader.ReadToEnd());
                var properties = typeof(StorageFileContext).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    TableNameAttribute? tableName = property.GetCustomAttribute<TableNameAttribute>();
                    if (tableName != null && ValidateProperty(property))
                    {
                        SetIgnoreAttribute(property);
                        try
                        {
                            JToken list = data[tableName.Name];
                            property.SetValue(this, list.ToObject(property.PropertyType));
                        }
                        catch
                        {
                            property.SetValue(this, property.PropertyType.GetConstructors()[0].Invoke(null));
                            indexes.TryAdd(tableName.Name, 1);
                            sets.Add(property);
                            continue;
                        }
                        try
                        {
                            JToken list = data[$"{tableName.Name}_index"];
                            indexes.TryAdd(tableName.Name, list.ToObject<int>());
                        }
                        catch
                        {
                            indexes.TryAdd(tableName.Name, GetId(property));
                        }
                        sets.Add(property);
                    }
                }
            }
            SetFK();
        }

        /// <summary>
        /// Получение таблицы данных по типу сущности
        /// </summary>
        internal List<TEntity> Set<TEntity>()
        {
            PropertyInfo property = sets.FirstOrDefault(x => x.PropertyType == typeof(List<TEntity>));
            return property.GetValue(this) as List<TEntity>;
        }

        /// <summary>
        /// Запись данных в долговременное хранилище
        /// </summary>
        internal void SaveChanges()
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = jsonResolver;
            using (StreamWriter writer = new StreamWriter(path))
            {
                JObject data = new JObject();
                foreach (PropertyInfo property in sets)
                {
                    TableNameAttribute? tableName = property.GetCustomAttribute<TableNameAttribute>();
                    if (tableName != null)
                    {
                        SetId(property, tableName.Name);
                        JsonConvert.DefaultSettings = () => serializerSettings;
                        data.Add(tableName.Name, JToken.FromObject(property.GetValue(this)));
                        data.Add($"{tableName.Name}_index", JToken.FromObject(indexes[tableName.Name]));
                    }
                }
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, data);
            }
            SetFK();
        }

        /// <summary>
        /// Расчет потенциально адекватного Id на случай повреждения хранилища
        /// </summary>
        /// <param name="property">Таблица для которой утеряны данные об текущем идентификаторе</param>
        /// <returns>Новый текущий Id</returns>
        private int GetId(PropertyInfo property)
        {
            int max = 1;
            if (property.GetValue(this) is IList list)
            {
                foreach (object value in list)
                {
                    if (value.GetType().GetProperty("Id").GetValue(value) is int now && max <= now)
                    {
                        max = now + 1;
                    }
                }
            }
            return max;
        }

        /// <summary>
        /// Автоинеремент первичного ключа
        /// </summary>
        /// <param name="property">Таблица для которой расчитыывается автоинкркемент</param>
        /// <param name="name">Имя таблицы для которой расчитыывается автоинкркемент</param>
        private void SetId(PropertyInfo property, string name)
        {
            if (property.GetValue(this) is IList list)
            {
                foreach (object value in list)
                {
                    if (value.GetType().GetProperty("Id").GetValue(value) is int id && id == 0)
                    {
                        value.GetType().GetProperty("Id").SetValue(value, indexes[name]);
                        indexes[name]++;
                    }
                }
            }
        }

        /// <summary>
        /// Валидация свойства на возможность быть таблицей. Допускаются списки элементов, содержащих целочисленное поле Id
        /// </summary>
        /// <param name="property"></param>
        private bool ValidateProperty(PropertyInfo property)
        {
            return property.PropertyType.IsGenericType
                        && property.PropertyType.GenericTypeArguments.Length == 1
                        && property.PropertyType.GenericTypeArguments[0].GetProperty("Id")?.PropertyType == typeof(int);
        }

        /// <summary>
        /// Обновлаение навигационных свойств сущностей
        /// </summary>
        private void SetFK()
        {
            if (oneToMany == null) oneToMany = new List<IList>();
            else
            {
                oneToMany.ForEach(x => x.Clear());
                oneToMany.Clear();
            }
            foreach (var set in sets)
            {
                FKAttribute fK = set.GetCustomAttribute<FKAttribute>();
                if (fK != null)
                {
                    if (set.GetValue(this) is IList fKList)
                    {
                        foreach (object fKElement in fKList)
                        {
                            object fKValue = fKElement.GetType().GetProperty(fK.FK).GetValue(fKElement);
                            PropertyInfo relation = sets.FirstOrDefault(x => x.PropertyType.GenericTypeArguments[0] == fK.Relation);
                            if (relation.GetValue(this) is IList pKList)
                            {
                                foreach (object pKElement in pKList)
                                {
                                    object pKValue = pKElement.GetType().GetProperty(fK.PK).GetValue(pKElement);
                                    if (fKValue.Equals(pKValue))
                                    {
                                        if (fK.FKNavigation != null)
                                            fKElement.GetType().GetProperty(fK.FKNavigation).SetValue(fKElement, pKElement);
                                        if (fK.PKNavigation != null)
                                        {
                                            if (pKElement.GetType().GetProperty(fK.PKNavigation).GetValue(pKElement) is IList pKNavigation)
                                            {
                                                pKNavigation.Add(fKElement);
                                                oneToMany.Add(pKNavigation);
                                            }
                                            else
                                            {
                                                pKElement.GetType().GetProperty(fK.PKNavigation).SetValue(pKElement, fKElement);
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SetIgnoreAttribute(PropertyInfo property)
        {
            JsonIgnoreStorageAttribute? jsonIgnore = property.GetCustomAttribute<JsonIgnoreStorageAttribute>();
            if (jsonIgnore != null)
            {
                jsonResolver.IgnoreProperty(property.PropertyType.GenericTypeArguments[0], jsonIgnore.PropertyNames);
            }
        }
    }
}
