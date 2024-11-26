using Storage.Data.Models.Box;
using Storage.Data.Models.Input;

namespace Storage.BusinessLogic.Service;
/// <summary>
/// Сервис CRUD для таблицы коробок
/// </summary>
public interface IBoxService
{
    /// <summary>
    /// Добавление коробки в хранилище
    /// </summary>
    public Task Add(BoxRequest boxDto);

    /// <summary>
    /// Получение информации о корбоках из хранилища
    /// </summary>
    public Task<IReadOnlyCollection<BoxPrintModel>> GetAll();

    /// <summary>
    /// Обновление информации о корбоках
    /// </summary>
    /// <param name="boxDto">Новая модель данных</param>
    /// <param name="id">Id изменяемой коробки</param>
    public Task Update(BoxRequest boxDto, int id);

    /// <summary>
    /// Удаление коробки
    /// </summary>
    /// <param name="id">Id удалемой коробки</param>
    public Task Delete(int id);
}
