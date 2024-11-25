using AutoMapper;
using Storage.Bl.Service.Interfaces;
using Storage.Data.Entity;
using Storage.Data.Models.Input;
using Storage.Data.Models.Output;
using Storage.Data.Repositories;

namespace Storage.Bl.Service
{
    public class PalletService : IPalletService
    {
        IStorageRepository storageRepository;
        IMapper mapper;

        public class AutoMapProfile : Profile
        {
            public AutoMapProfile()
            {
                CreateMap<PalletInputDto, Pallet>()
                    .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                    .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                    .ForMember(dest => dest.Deep, opt => opt.MapFrom(src => src.Deep));
                CreateMap<Pallet, PalletOutputDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.GetWeight()))
                    .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.GetVolume()))
                    .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                    .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                    .ForMember(dest => dest.Deep, opt => opt.MapFrom(src => src.Deep))
                    .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.GetExpirationDate()))
                    .AfterMap((src, dest, context) => dest.Boxes = context.Mapper.Map<BoxOutputDto[]>(src.RealBoxes));
            }
        }

        public PalletService(IStorageRepository storageRepository,
                IMapper mapper)
        {
            this.storageRepository = storageRepository;
            this.mapper = mapper;
        }

        public async Task Add(PalletInputDto palletDto)
        {
            await storageRepository.Add(mapper.Map<PalletInputDto, Pallet>(palletDto));
        }

        public async Task<Pallet> GetById(int id)
            => await storageRepository.GetOne<Pallet>(x => x.Id == id, [x => x.Boxes]);

        public async Task<PalletOutputDto[]> GetAll()
            => mapper.Map<PalletOutputDto[]>((await storageRepository.Get<Pallet>(null, null, null, null, [x => x.Boxes])).ToList());


        public async Task Update(PalletInputDto palletDto, int id)
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

        public async Task<List<List<PalletOutputDto>>> GetSortedPallet((PalletInputDto[], BoxInputDto[])? dataSourse = null)
        {
            IQueryable<Pallet> pallets;
            if (dataSourse.HasValue)
                pallets = PrepareData(dataSourse.Value).Item1;
            else
                pallets = await storageRepository.Get<Pallet>(null, null, null, null, [x => x.Boxes]);
            var grouped = pallets.GroupBy(x => x.Boxes.Where(y => !y.DeletedDate.HasValue).Min(y => (y.ExpirationDate)));
            var selected = grouped.Select(x => new
            {
                x.Key,
                Value = x.OrderBy(y => y.Boxes.Where(z => !z.DeletedDate.HasValue)
                                                .Sum(z => z.Weight) + Constants.BASE_PALLET_WEIGHT).ToList()
            });
            return selected.OrderBy(x => x.Key).ToList().ConvertAll(x => mapper.Map<List<PalletOutputDto>>(x.Value));
        }

        public async Task<List<PalletOutputDto>> GetLongestLifePallets((PalletInputDto[], BoxInputDto[])? dataSourse = null)
        {
            IQueryable<Pallet> pallets;
            if (dataSourse.HasValue)
                pallets = PrepareData(dataSourse.Value).Item1;
            else
                pallets = await storageRepository.Get<Pallet>(null, null, null, null, [x => x.Boxes]);
            var ordered = pallets.OrderByDescending(x => x.Boxes.Where(y => !y.DeletedDate.HasValue).Min(y => y.ExpirationDate));
            return mapper.Map<List<PalletOutputDto>>(ordered.Take(3)
                .OrderBy(x => x.Width * x.Height * x.Deep 
                        + x.Boxes.Where(y => !y.DeletedDate.HasValue).Sum(y => y.Width * y.Height * y.Deep)).ToList());
        }

        private (IQueryable<Pallet>, IQueryable<Box>) PrepareData((PalletInputDto[], BoxInputDto[]) dataSource)
        {
            Pallet[] pallets = mapper.Map<Pallet[]>(dataSource.Item1);
            Box[] boxes = mapper.Map<Box[]>(dataSource.Item2);
            for (int i = 0; i < pallets.Length; i++)
            {
                pallets[i].Id = i + 1;
            }
            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i].Id = i + 1;
                boxes[i].Pallet = pallets[boxes[i].PalletId];
                boxes[i].Pallet.Boxes.Add(boxes[i]);
            }
            return (pallets.AsQueryable(), boxes.AsQueryable());
        }
    }
}
