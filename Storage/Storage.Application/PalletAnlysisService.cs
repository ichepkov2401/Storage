using Storage.Data.Models.Pallet;
using Storage.Data.Repositories;

namespace Storage.Application
{
    internal class PalletAnlysisService : IPalletAnalysisService
    {
        IStorageRepository storageRepository;

        internal PalletAnlysisService(IStorageRepository storageRepository)
        {
            this.storageRepository = storageRepository;
        }

        /// <summary>
        /// Сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу.
        /// </summary>
        public Task<IReadOnlyCollection<(DateTime?, IReadOnlyList<PalletPrintModel>)>> GetSortedPallet()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.
        /// </summary>
        public Task<IReadOnlyCollection<PalletPrintModel>> GetLongestLifePallets()
        {
            throw new NotImplementedException();
        }
    }
}
