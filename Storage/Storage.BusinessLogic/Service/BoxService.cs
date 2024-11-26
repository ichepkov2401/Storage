using AutoMapper;
using Storage.Data.Entity;
using Storage.Data.Models.Box;
using Storage.Data.Models.Input;
using Storage.Data.Repositories;

namespace Storage.BusinessLogic.Service;

public class BoxService : IBoxService
{
    IStorageRepository storageRepository;
    IPalletService palletService;
    IMapper mapper;

    public class AutoMapProfile : Profile
    {
        public AutoMapProfile()
        {
            CreateMap<BoxRequest, Box>()
                .ForMember(dest => dest.PalletId, opt => opt.MapFrom(src => src.PalletId))
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.ExpirationDate))
                .ForMember(dest => dest.ProductionDate, opt => opt.MapFrom(src => src.ProductionDate))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Depth, opt => opt.MapFrom(src => src.Depth))
                .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                .AfterMap((src, dest) => dest.ExpirationDate = dest.ExpirationDate); // Чтобы точно ExpirationDate был

            CreateMap<Box, BoxPrintModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PalletId, opt => opt.MapFrom(src => src.PalletId))
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.ExpirationDate))
                .ForMember(dest => dest.ProductionDate, opt => opt.MapFrom(src => src.ProductionDate))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Depth, opt => opt.MapFrom(src => src.Depth))
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

    public async Task Add(BoxRequest boxDto)
    {
        BoxDtoValidate(boxDto);
        Box box = mapper.Map<BoxRequest, Box>(boxDto);
        await storageRepository.Add(box);
    }

    public async Task<IReadOnlyCollection<BoxPrintModel>> GetAll()
        => mapper.Map<IReadOnlyCollection<BoxPrintModel>>((await storageRepository.Get<Box>()).ToList());

    public async Task Update(BoxRequest boxDto, int id)
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
    private async void BoxDtoValidate(BoxRequest boxDto)
    {
        if (!boxDto.ProductionDate.HasValue && !boxDto.ExpirationDate.HasValue)
            throw new ArgumentException("У коробки должен быть указан срок годности или дата производства.");
        Pallet pallet = await palletService.GetById(boxDto.PalletId);
        if (pallet.Width < boxDto.Width || pallet.Depth < boxDto.Depth)
            throw new ArgumentException("Коробка не должна превышать по размерам паллету (по ширине и глубине).");
        boxDto.ProductionDate = boxDto.ProductionDate?.Date;
        boxDto.ExpirationDate = boxDto.ExpirationDate?.Date;
    }

    private async Task<Box> GetById(int id)
        => await storageRepository.GetOne<Box>(x => x.Id == id);
}