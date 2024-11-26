using Storage.Data.Entity;
using Storage.Data.Models.Input;
using Storage.Data.Models.Pallet;

namespace Storage.BusinessLogic.Service;
/// <summary>
/// Сервис CRUD и анализа паллетов
/// </summary>
public interface IPalletService
{
    public Task Add(PalletRequest palletDto);

    public Task<Pallet> GetById(int id);

    public Task<IReadOnlyCollection<PalletPrintModel>> GetAll();


    public Task Update(PalletRequest palletDto, int id);

    public Task Delete(int id);

    /// <summary>
    /// Сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу.
    /// </summary>
    public Task<IReadOnlyCollection<(DateTime?, IReadOnlyList<PalletPrintModel>)>> GetSortedPallet((PalletRequest[], BoxRequest[])? dataSource = null);


    /// <summary>
    /// 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.
    /// </summary>
    public Task<IReadOnlyCollection<PalletPrintModel>> GetLongestLifePallets((PalletRequest[], BoxRequest[])? dataSource = null);
}
