using Storage.Data.Entity;
using Storage.Data.Models.Input;
using Storage.Data.Models.Output;

namespace Storage.BusinessLogic.Service.Interfaces
{
    /// <summary>
    /// Сервис CRUD и анализа паллетов
    /// </summary>
    public interface IPalletService
    {
        public Task Add(PalletRequest palletDto);

        public Task<Pallet> GetById(int id);

        public Task<IReadOnlyCollection<PalletResponse>> GetAll();


        public Task Update(PalletRequest palletDto, int id);

        public Task Delete(int id);

        /// <summary>
        /// Сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу.
        /// </summary>
        public Task<IReadOnlyCollection<IGrouping<DateTime?, PalletResponse>>> GetSortedPallet((PalletRequest[], BoxRequest[])? dataSource = null);


        /// <summary>
        /// 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.
        /// </summary>
        public Task<IReadOnlyCollection<PalletResponse>> GetLongestLifePallets((PalletRequest[], BoxRequest[])? dataSource = null);
    }
}
