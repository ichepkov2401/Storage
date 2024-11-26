using Storage.Data.Models.Pallet;

namespace Storage.Data.Repositories;
public interface IStorageRepository
{
    /// <summary>
    /// Сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу.
    /// </summary>
    public IReadOnlyCollection<(DateTime? key, IReadOnlyList<PalletStoreModel> values)> GetSortedPallet();


    /// <summary>
    /// 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.
    /// </summary>
    public IReadOnlyCollection<PalletStoreModel> GetLongestLifePallets();
}

