namespace Storage.Data
{
    /// <summary>
    /// Атрибут имени таблицы, свойство помоеченное данным атрибутом будет рассматриваться как таблица в JSON хранилище
    /// </summary>
    internal class TableNameAttribute : Attribute
    {
        public string Name { get; }

        public TableNameAttribute(string name)
        {
            Name = name;
        }

    }
}
