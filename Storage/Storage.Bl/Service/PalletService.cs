using AutoMapper;
using Storage.Bl.Service.Interfaces;
using Storage.Data.Entity;
using Storage.Data.Models;
using Storage.Data.Repositories;
using System.Linq.Expressions;

namespace Storage.Bl.Service
{
    public class PalletService : IPalletService
    {
        IStorageRepository storageRepository;
        IMapper mapper;

        public PalletService(IStorageRepository storageRepository,
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
            => await storageRepository.GetOne<Pallet>(x => x.Id == id, [x => x.Boxes]);

        public async Task<IQueryable<Pallet>> GetAll()
            => await storageRepository.Get<Pallet>(null, null, null, null, [x => x.Boxes]);


        public async Task Update(PalletDto palletDto, int id)
        {
            Pallet pallet = await GetById(id);
            mapper.Map(palletDto, pallet);
            await storageRepository.Update(pallet);
        }

        public async Task Delete(int id)
        {
            Pallet pallet = await GetById(id);
            if (pallet.RealBoxes.Count() > 0)
                throw new ArgumentException("Нельзя убрать со склада паллет, пока на нем стоят коробки");
            await storageRepository.Delete(pallet);
        }

        public async Task<List<List<Pallet>>> GetSortedPallet(List<Pallet> pallets = null)
        {
            if (pallets == null)
                pallets = (await storageRepository.Get<Pallet>(null, null, null, null, [x => x.Boxes])).ToList();
            return pallets.GroupBy(x => x.ExpirationDate)
                .OrderBy(x => x.Key)
                .Select(x => x.OrderBy(y => y.Weight).ToList()).ToList();
        }

        public async Task<List<Pallet>> GetLongestLifePallets()
        {
            return (await storageRepository.Get<Pallet>(null, null, null, null, [x => x.Boxes])).ToList()
                .OrderByDescending(y => y.ExpirationDate).Take(3)
                .OrderBy(x => x.Volume).ToList();
        }

        public List<Pallet> GetLongestLifePallets(List<Pallet> pallets)
        {
            return pallets.OrderByDescending(y => y.ExpirationDate).Take(3).OrderBy(x => x.Volume).ToList();
        }
    }
}
