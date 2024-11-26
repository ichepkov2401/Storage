using System.Linq.Expressions;
using AutoMapper;
using Moq;
using Storage.BusinessLogic.Service;
using Storage.Data.Entity;
using Storage.Data.Models.Pallet;
using Storage.Data.Repositories;

namespace Storage.Test
{
    [TestFixture]
    public class PalletServiceTest
    {
        Mock<IMapper> mapper;

        [SetUp]
        public void Setup()
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

        [Test]
        public async Task SortedPalletEmptyTest()
        {
            Mock<IStorageRepository> mockRepository = new Mock<IStorageRepository>();
            mockRepository.Setup(m => m.Get<Pallet>(null, null, null, null, null)).Returns(new Task<IQueryable<Pallet>>(() => new List<Pallet>() { }.AsQueryable()));
            PalletService palletService = new PalletService(mockRepository.Object, mapper.Object);

            var result = await palletService.GetSortedPallet();

            CollectionAssert.AreEqual(new List<List<Pallet>>(), result);
        }

        [Test]
        public async Task SortedPalletOneElementTest()
        {
            Mock<IStorageRepository> mockRepository = new Mock<IStorageRepository>();
            Pallet pallet = new Pallet() { Id = 1 };
            var includes = new List<Expression<Func<Pallet, object>>>() { x => x.Boxes };
            mockRepository.Setup(m => m.Get(null, null, null, null, It.IsAny<ICollection<Expression<Func<Pallet, object>>>>())).Returns(
                async () => new List<Pallet>() { pallet }.AsQueryable());
            PalletService palletService = new PalletService(mockRepository.Object, mapper.Object);

            var result = await palletService.GetSortedPallet();

            Assert.That(result.Count, Is.EqualTo(1));
        }


        [Test]
        public async Task SortedPalletDateTest()
        {
            Mock<IStorageRepository> mockRepository = new Mock<IStorageRepository>();
            Pallet pallet1 = new Pallet() { Id = 1 };
            Pallet pallet2 = new Pallet() { Id = 2 };
            Pallet pallet3 = new Pallet() { Id = 3 };
            pallet1.Boxes.Add(new Box() { ExpirationDate = new DateTime(2001, 1, 1) });
            pallet2.Boxes.Add(new Box() { ExpirationDate = new DateTime(2001, 1, 1) });
            pallet2.Boxes.Add(new Box() { ExpirationDate = new DateTime(2002, 1, 1) });
            pallet3.Boxes.Add(new Box() { ProductionDate = new DateTime(2000, 1, 1), ExpirationDate = new DateTime(2000, 1, 2) });
            pallet3.Boxes.Add(new Box() { ExpirationDate = new DateTime(2002, 1, 1) });
            pallet3.Boxes.Add(new Box() { ExpirationDate = new DateTime(2003, 1, 1) });
            var includes = new List<Expression<Func<Pallet, object>>>() { x => x.Boxes };
            mockRepository.Setup(m => m.Get(null, null, null, null, It.IsAny<ICollection<Expression<Func<Pallet, object>>>>())).Returns(
                async () => new List<Pallet>() { pallet1, pallet2, pallet3 }.AsQueryable());
            PalletService palletService = new PalletService(mockRepository.Object, mapper.Object);

            var result = await palletService.GetSortedPallet();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task SortedPalletWeightTest()
        {
            Mock<IStorageRepository> mockRepository = new Mock<IStorageRepository>();
            Pallet pallet1 = new Pallet() { Id = 1 };
            Pallet pallet2 = new Pallet() { Id = 2 };
            Pallet pallet3 = new Pallet() { Id = 3 };
            pallet1.Boxes.Add(new Box() { ExpirationDate = new DateTime(2001, 1, 1), Weight = 60 });
            pallet2.Boxes.Add(new Box() { ExpirationDate = new DateTime(2001, 1, 1), Weight = 20 });
            pallet2.Boxes.Add(new Box() { ExpirationDate = new DateTime(2002, 1, 1), Weight = 30 });
            pallet3.Boxes.Add(new Box() { ProductionDate = new DateTime(2000, 1, 1), ExpirationDate = new DateTime(2000, 1, 2), Weight = 40 });
            pallet3.Boxes.Add(new Box() { ExpirationDate = new DateTime(2002, 1, 1), Weight = 50 });
            pallet3.Boxes.Add(new Box() { ExpirationDate = new DateTime(2003, 1, 1), Weight = 60 });
            var includes = new List<Expression<Func<Pallet, object>>>() { x => x.Boxes };
            mockRepository.Setup(m => m.Get(null, null, null, null, It.IsAny<ICollection<Expression<Func<Pallet, object>>>>())).Returns(
                async () => new List<Pallet>() { pallet1, pallet2, pallet3 }.AsQueryable());
            PalletService palletService = new PalletService(mockRepository.Object, mapper.Object);

            var result = await palletService.GetSortedPallet();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task LongLIfePalletTest()
        {
            Mock<IStorageRepository> mockRepository = new Mock<IStorageRepository>();
            Pallet pallet1 = new Pallet() { Width = 1, Height = 1, Depth = 1 };
            Pallet pallet2 = new Pallet();
            Pallet pallet3 = new Pallet();
            Pallet pallet4 = new Pallet();
            Pallet pallet5 = new Pallet() { Width = 10, Height = 5, Depth = 2 };
            pallet1.Boxes.Add(new Box() { ExpirationDate = new DateTime(2001, 1, 1), Height = 1, Width = 1, Depth = 1 });
            pallet2.Boxes.Add(new Box() { ExpirationDate = new DateTime(2001, 1, 1), Height = 2, Width = 3, Depth = 5 });
            pallet2.Boxes.Add(new Box() { ExpirationDate = new DateTime(2002, 1, 1), Height = 3, Width = 5, Depth = 7 });
            pallet3.Boxes.Add(new Box() { ProductionDate = new DateTime(2000, 1, 1), ExpirationDate = new DateTime(2000, 1, 1) });
            pallet3.Boxes.Add(new Box() { ExpirationDate = new DateTime(2002, 1, 1) });
            pallet3.Boxes.Add(new Box() { ExpirationDate = new DateTime(2003, 1, 1) });
            pallet4.Boxes.Add(new Box() { ExpirationDate = new DateTime(2010, 1, 1) });
            pallet4.Boxes.Add(new Box() { ProductionDate = new DateTime(2000, 1, 1), ExpirationDate = new DateTime(2000, 1, 1) });
            pallet5.Boxes.Add(new Box() { ExpirationDate = new DateTime(2010, 1, 1) });
            pallet5.Boxes.Add(new Box() { ExpirationDate = new DateTime(2010, 1, 1) });
            var includes = new List<Expression<Func<Pallet, object>>>() { x => x.Boxes };
            mockRepository.Setup(m => m.Get(null, null, null, null, It.IsAny<ICollection<Expression<Func<Pallet, object>>>>())).Returns(
                async () => new List<Pallet>() { pallet1, pallet2, pallet3, pallet4, pallet5 }.AsQueryable());
            PalletService palletService = new PalletService(mockRepository.Object, mapper.Object);

            var result = await palletService.GetLongestLifePallets();

            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result.ElementAt(0).Volume, Is.EqualTo(2));
            Assert.That(result.ElementAt(1).Volume, Is.EqualTo(100));
            Assert.That(result.ElementAt(2).Volume, Is.EqualTo(135));
        }
    }
}
