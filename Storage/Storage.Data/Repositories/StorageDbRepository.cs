using Storage.Data.Contexts;
using Storage.Data.Models.Pallet;

namespace Storage.Data.Repositories;
public class StorageDbRepository : IStorageRepository
{
    private readonly StorageDbContext context;

    public StorageDbRepository(string connection)
    {
        context = new StorageDbContext(connection);
    }

    public IReadOnlyCollection<PalletStoreModel> GetLongestLifePallets()
    {
        return context.Pallets.OrderByDescending(x => x.Boxes.Min(y => y.ExpirationDate))
            .Take(3).OrderBy(x => x.Width * x.Height * x.Depth + x.Boxes
            .Sum(y => y.Width * y.Height * y.Depth)).ToList();
    }

    public IReadOnlyCollection<(DateTime? key, IReadOnlyList<PalletStoreModel> values)> GetSortedPallet()
    {
        return context.Pallets.GroupBy(x => x.Boxes.Min(y => (y.ExpirationDate)))
            .Select(x => new
            {
                x.Key,
                Value = x.OrderBy(y => y.Boxes.Sum(z => z.Weight) + 30).ToList()
            })
            .OrderBy(x => x.Key).ToList()
            .ConvertAll<(DateTime?, IReadOnlyList<PalletStoreModel>)>(x => (x.Key, x.Value));
    }
}
