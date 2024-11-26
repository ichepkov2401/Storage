using Storage.Data.Models.Pallet;

namespace Storage.Application
{
    public interface IPalletAnalysisService
    {
        /// <summary>
        /// Сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу.
        /// </summary>
        public Task<IReadOnlyCollection<(DateTime?, IReadOnlyList<PalletPrintModel>)>> GetSortedPallet();


        /// <summary>
        /// 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.
        /// </summary>
        public Task<IReadOnlyCollection<PalletPrintModel>> GetLongestLifePallets();
    }
}
