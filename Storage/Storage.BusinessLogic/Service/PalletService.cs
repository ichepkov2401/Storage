using AutoMapper;
using Storage.Data.Entity;
using Storage.Data.Models.Box;
using Storage.Data.Models.Input;
using Storage.Data.Models.Pallet;
using Storage.Data.Repositories;

namespace Storage.BusinessLogic.Service;
public class PalletService : IPalletService
{
    IStorageRepository storageRepository;
    IMapper mapper;

    public class AutoMapProfile : Profile
    {
        public AutoMapProfile()
        {
            CreateMap<PalletRequest, Pallet>()
                .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Depth, opt => opt.MapFrom(src => src.Deep));
            CreateMap<Pallet, PalletPrintModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.GetWeight()))
                .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.GetVolume()))
                .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Deep, opt => opt.MapFrom(src => src.Depth))
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.GetExpirationDate()))
                .AfterMap((src, dest, context) => dest.Boxes = context.Mapper.Map<BoxPrintModel[]>(src.RealBoxes));
        }
    }

    public PalletService(IStorageRepository storageRepository,
            IMapper mapper)
    {
        this.storageRepository = storageRepository;
        this.mapper = mapper;
    }

    public async Task Add(PalletRequest palletDto)
    {
        await storageRepository.Add(mapper.Map<PalletRequest, Pallet>(palletDto));
    }

    public async Task<Pallet> GetById(int id)
        => await storageRepository.GetOne<Pallet>(x => x.Id == id, [x => x.Boxes]);

    public async Task<IReadOnlyCollection<PalletPrintModel>> GetAll()
        => mapper.Map<PalletPrintModel[]>((await storageRepository.Get<Pallet>(null, null, null, null, [x => x.Boxes])).ToList());


    public async Task Update(PalletRequest palletDto, int id)
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

    public async Task<IReadOnlyCollection<(DateTime?, IReadOnlyList<PalletPrintModel>)>> GetSortedPallet((PalletRequest[], BoxRequest[])? dataSourse = null)
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
                                            .Sum(z => z.Weight) + 30).ToList()
        });
        return selected.OrderBy(x => x.Key)
                            .ToList().ConvertAll<(DateTime?, IReadOnlyList<PalletPrintModel>)>(x
                                => (x.Key, mapper.Map<List<PalletPrintModel>>(x.Value)));
    }

    public async Task<IReadOnlyCollection<PalletPrintModel>> GetLongestLifePallets((PalletRequest[], BoxRequest[])? dataSourse = null)
    {
        IQueryable<Pallet> pallets;
        if (dataSourse.HasValue)
            pallets = PrepareData(dataSourse.Value).Item1;
        else
            pallets = await storageRepository.Get<Pallet>(null, null, null, null, [x => x.Boxes]);
        var ordered = pallets.OrderByDescending(x => x.Boxes.Where(y => !y.DeletedDate.HasValue).Min(y => y.ExpirationDate));


        var v = ordered.Take(3)
            .OrderBy(x => x.Width * x.Height * x.Depth
                    + x.Boxes.Where(y => !y.DeletedDate.HasValue).Sum(y => y.Width * y.Height * y.Depth));

        return mapper.Map<List<PalletPrintModel>>(ordered.Take(3)
            .OrderBy(x => x.Width * x.Height * x.Depth
                    + x.Boxes.Where(y => !y.DeletedDate.HasValue).Sum(y => y.Width * y.Height * y.Depth)).ToList());
    }

    private (IQueryable<Pallet>, IQueryable<Box>) PrepareData((PalletRequest[], BoxRequest[]) dataSource)
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
