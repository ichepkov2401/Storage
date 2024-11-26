using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Storage.BusinessLogic.Service;
using Storage.Data.Entity;
using Storage.Data.Models.Pallet;
using Storage.Data.Repositories;

namespace Storage.TestX
{
    public class PalletServiceTest
    {
        Mock<IMapper> mapper;


        public void MapperSetUp()
        {
            mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<List<PalletPrintModel>>(It.IsAny<List<Pallet>>())).Returns<List<Pallet>>(
                x =>
                {
                    List<PalletPrintModel> res = new List<PalletPrintModel>();
                    foreach (var item in x)
                    {
                        res.Add(new PalletPrintModel()
                        {
                            Id = item.Id,
                            Deep = item.Depth,
                            Weight = item.RealBoxes.Sum(x => x.Weight) + 30,
                            Width = item.Width,
                            Height = item.Height,
                            Volume = item.RealBoxes.Sum(x => x.Width * x.Height * x.Depth) + item.Width * item.Height * item.Depth,
                            ExpirationDate = item.RealBoxes.Min(x => x.ExpirationDate)
                        });
                    }
                    return res;
                });
        }

        [Fact]
        public async void Test1()
        {
            Mock<IStorageRepository> mockRepository = new Mock<IStorageRepository>();
            mockRepository.Setup(m => m.Get<Pallet>(null, null, null, null, null)).Returns(new Task<IQueryable<Pallet>>(() => new List<Pallet>() { }.AsQueryable()));
            PalletService palletService = new PalletService(mockRepository.Object, mapper.Object);

            var result = await palletService.GetSortedPallet();

            Assert.True(result.Count == 1);
        }
    }
}