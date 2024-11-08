using AutoMapper;
using Storage.Bl.Service.Interfaces;
using Storage.Data.Entity;
using Storage.Data.Models;
using Storage.Data.Repositories;

namespace Storage.Bl.Service
{
    public class BoxService : IBoxService
    {
        IStorageRepository storageRepository;
        IPalletService palletService;
        IMapper mapper;

        public BoxService(IStorageRepository storageRepository,
            IPalletService palletService,
            IMapper mapper)
        {
            this.storageRepository = storageRepository;
            this.palletService = palletService;
            this.mapper = mapper;
        }

        public async Task Add(BoxDto boxDto)
        {
            BoxDtoValidate(boxDto);
            Box box = mapper.Map<BoxDto, Box>(boxDto);
            await storageRepository.Add(box);
        }

        public async Task<Box> GetById(int id)
            => await storageRepository.GetOne<Box>(x => x.Id == id);

        public async Task<IQueryable<Box>> GetAll()
            => await storageRepository.Get<Box>();

        public async Task Update(BoxDto boxDto, int id)
        {
            BoxDtoValidate(boxDto);
            Box box = await GetById(id);
            mapper.Map(boxDto, box);
            await storageRepository.Update(box);
        }

        public async Task Delete(int id)
            => await storageRepository.Delete(await GetById(id));

        private async void BoxDtoValidate(BoxDto boxDto)
        {
            if (!boxDto.ProductionDate.HasValue && !boxDto.ExpirationDate.HasValue)
                throw new ArgumentException("У коробки должен быть указан срок годности или дата производства.");
            Pallet pallet = await palletService.GetById(boxDto.PalletId);
            if (pallet.Width < boxDto.Width || pallet.Deep < boxDto.Deep)
                throw new ArgumentException("Коробка не должна превышать по размерам паллету (по ширине и глубине).");
            boxDto.ProductionDate = boxDto.ProductionDate?.Date;
            boxDto.ExpirationDate = boxDto.ExpirationDate?.Date;
        }
    }
}
