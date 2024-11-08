using AutoMapper;
using Storage.Bl.Service.Interfaces;
using Storage.Data.Entity;
using Storage.Data.Models;
using Storage.Data.Repositories;

namespace Storage.Bl.Service
{
    class PalletService : IPalletService
    {
        IStorageRepository storageRepository;
        IMapper mapper;

        PalletService(IStorageRepository storageRepository,
            IMapper mapper)
        {
            this.storageRepository = storageRepository;
            this.mapper = mapper;
        }

        public async Task Add(PalletDto palletDto)
        {
            await storageRepository.Add(mapper.Map<PalletDto, Pallet>(palletDto));
        }

        public async Task<Pallet> GetById(int id)
            => await storageRepository.GetOne<Pallet>(x => x.Id == id);

        public async Task<IQueryable<Pallet>> GetAll()
            => await storageRepository.Get<Pallet>();


        public async Task Update(PalletDto palletDto, int id)
        {
            Pallet pallet = await GetById(id);
            mapper.Map(palletDto, pallet);
            await storageRepository.Update(pallet);
        }

        public async Task Delete(int id)
            => await storageRepository.Delete(await GetById(id));

        public async Task<IQueryable<IOrderedEnumerable<Pallet>>> GetSortedPallet(IQueryable<Pallet> pallets = null)
        {
            if (pallets == null)
                pallets = await storageRepository.Get<Pallet>();
            return pallets.GroupBy(x => x.ExpirationDate)
                .OrderBy(x => x.Key)
                .Select(x => x.OrderBy(y => y.Weight));
        }

        public async Task<IOrderedQueryable<Pallet>> GetLongestLifePallets()
        {
            return (await storageRepository.Get<Pallet>(null,
                x => x.OrderByDescending(y => y.ExpirationDate), 0, 3))
                .OrderBy(x => x.Volume);
        }

        public IOrderedQueryable<Pallet> GetLongestLifePallets(IQueryable<Pallet> pallets)
        {
            return pallets.OrderByDescending(y => y.ExpirationDate).Take(3).OrderBy(x => x.Volume);
        }
    }
}
