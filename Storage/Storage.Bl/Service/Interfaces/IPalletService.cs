using Storage.Data.Entity;
using Storage.Data.Models.Input;
using Storage.Data.Models.Output;

namespace Storage.Bl.Service.Interfaces
{
    public interface IPalletService
    {
        public Task Add(PalletInputDto palletDto);

        public Task<Pallet> GetById(int id);

        public Task<PalletOutputDto[]> GetAll();


        public Task Update(PalletInputDto palletDto, int id);

        public Task Delete(int id);

        /// <summary>
        /// Сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу.
        /// </summary>
        public Task<List<List<PalletOutputDto>>> GetSortedPallet((PalletInputDto[], BoxInputDto[])? dataSource = null);


        /// <summary>
        /// 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.
        /// </summary>
        public Task<List<PalletOutputDto>> GetLongestLifePallets((PalletInputDto[], BoxInputDto[])? dataSource = null);
    }
}
