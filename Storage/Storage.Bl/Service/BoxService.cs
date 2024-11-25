using AutoMapper;
using Storage.Bl.Service.Interfaces;
using Storage.Data.Entity;
using Storage.Data.Models.Input;
using Storage.Data.Models.Output;
using Storage.Data.Repositories;

namespace Storage.Bl.Service
{
    public class BoxService : IBoxService
    {
        IStorageRepository storageRepository;
        IPalletService palletService;
        IMapper mapper;

        public class AutoMapProfile : Profile
        {
            public AutoMapProfile()
            {
                CreateMap<BoxInputDto, Box>()
                    .ForMember(dest => dest.PalletId, opt => opt.MapFrom(src => src.PalletId))
                    .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.ExpirationDate))
                    .ForMember(dest => dest.ProductionDate, opt => opt.MapFrom(src => src.ProductionDate))
                    .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                    .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                    .ForMember(dest => dest.Deep, opt => opt.MapFrom(src => src.Deep))
                    .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                    .AfterMap((src, dest) => dest.ExpirationDate = dest.GetExpirationDate()); // Чтобы точно ExpirationDate был

                CreateMap<Box, BoxOutputDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.PalletId, opt => opt.MapFrom(src => src.PalletId))
                    .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.GetExpirationDate()))
                    .ForMember(dest => dest.ProductionDate, opt => opt.MapFrom(src => src.ProductionDate))
                    .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                    .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                    .ForMember(dest => dest.Deep, opt => opt.MapFrom(src => src.Deep))
                    .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                    .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.GetVolume()));
            }
        }

        public BoxService(IStorageRepository storageRepository,
            IPalletService palletService,
            IMapper mapper)
        {
            this.storageRepository = storageRepository;
            this.palletService = palletService;
            this.mapper = mapper;
        }

        public async Task Add(BoxInputDto boxDto)
        {
            BoxDtoValidate(boxDto);
            Box box = mapper.Map<BoxInputDto, Box>(boxDto);
            await storageRepository.Add(box);
        }

        public async Task<BoxOutputDto[]> GetAll()
            => mapper.Map<BoxOutputDto[]>((await storageRepository.Get<Box>()).ToList());

        public async Task Update(BoxInputDto boxDto, int id)
        {
            BoxDtoValidate(boxDto);
            Box box = await GetById(id);
            mapper.Map(boxDto, box);
            await storageRepository.Update(box);
        }

        public async Task Delete(int id)
            => await storageRepository.Delete(await GetById(id));

        /// <summary>
        /// Валидация модели коробки при ее созаднии
        /// </summary>
        /// <param name="boxDto">Модель коробки</param>
        /// <exception cref="ArgumentException">Модель коробки не соотвесвует требованиям БЛ</exception>
        private async void BoxDtoValidate(BoxInputDto boxDto)
        {
            if (!boxDto.ProductionDate.HasValue && !boxDto.ExpirationDate.HasValue)
                throw new ArgumentException("У коробки должен быть указан срок годности или дата производства.");
            Pallet pallet = await palletService.GetById(boxDto.PalletId);
            if (pallet.Width < boxDto.Width || pallet.Deep < boxDto.Deep)
                throw new ArgumentException("Коробка не должна превышать по размерам паллету (по ширине и глубине).");
            boxDto.ProductionDate = boxDto.ProductionDate?.Date;
            boxDto.ExpirationDate = boxDto.ExpirationDate?.Date;
        }

        private async Task<Box> GetById(int id)
            => await storageRepository.GetOne<Box>(x => x.Id == id);
    }
}
