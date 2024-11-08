using AutoMapper;
using Moq;
using Storage.Bl.Service;
using Storage.Data.Entity;
using Storage.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Test
{
    [TestFixture]
    public class PalletServiceTest
    {
        PalletService palletService;
        [SetUp]
        public void SetUp()
        {
            Mock<IStorageRepository> mockRepository = new Mock<IStorageRepository>();
            Mock<IMapper> mockmapper = new Mock<IMapper>();
            palletService = new PalletService(mockRepository.Object, mockmapper.Object);
        }

        [Test]
        public async Task SortedPalletEmptyTest()
        {
            Mock<IStorageRepository> mockRepository = new Mock<IStorageRepository>();
            mockRepository.Setup(m => m.Get<Pallet>(null, null, null, null, null)).Returns(new Task<IQueryable<Pallet>>(() => new List<Pallet>() { }.AsQueryable()));
            Mock<IMapper> mockmapper = new Mock<IMapper>();
            palletService = new PalletService(mockRepository.Object, mockmapper.Object);

            var result = await palletService.GetSortedPallet();

            CollectionAssert.AreEqual(new List<List<Pallet>>(), result);
        }

        [Test]
        public async Task SortedPalletOneElementTest()
        {
            Mock<IStorageRepository> mockRepository = new Mock<IStorageRepository>();
            Pallet pallet = new Pallet();
            var includes = new List<Expression<Func<Pallet, object>>>() { x => x.Boxes };
            mockRepository.Setup(m => m.Get(null, null, null, null, It.IsAny<ICollection<Expression<Func<Pallet, object>>>>())).Returns(
                async () => new List<Pallet>() { pallet }.AsQueryable());
            Mock<IMapper> mockmapper = new Mock<IMapper>();
            palletService = new PalletService(mockRepository.Object, mockmapper.Object);

            var result = await palletService.GetSortedPallet();

            Assert.AreEqual(1, result.Count);
            CollectionAssert.AreEqual(new List<Pallet>() { pallet }, result[0]);
        }


        [Test]
        public async Task SortedPalletDateTest()
        {
            Mock<IStorageRepository> mockRepository = new Mock<IStorageRepository>();
            Pallet pallet1 = new Pallet();
            Pallet pallet2 = new Pallet();
            Pallet pallet3 = new Pallet();
            pallet1.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2001, 1, 1) });
            pallet2.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2001, 1, 1) });
            pallet2.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2002, 1, 1) });
            pallet3.Boxes.Add(new Box() { ProductionDate = new DateTime(2000, 1, 1)});
            pallet3.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2002, 1, 1) });
            pallet3.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2003, 1, 1) });
            var includes = new List<Expression<Func<Pallet, object>>>() { x => x.Boxes };
            mockRepository.Setup(m => m.Get(null, null, null, null, It.IsAny<ICollection<Expression<Func<Pallet, object>>>>())).Returns(
                async () => new List<Pallet>() { pallet1, pallet2, pallet3 }.AsQueryable());
            Mock<IMapper> mockmapper = new Mock<IMapper>();
            palletService = new PalletService(mockRepository.Object, mockmapper.Object);

            var result = await palletService.GetSortedPallet();

            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEqual(new List<Pallet>() { pallet1, pallet2 }, result[1]);
            CollectionAssert.AreEqual(new List<Pallet>() { pallet3 }, result[0]);
            Assert.AreEqual(new DateTime(2000, 4, 10), result[0][0].ExpirationDate);
            Assert.AreEqual(new DateTime(2001, 1, 1), result[1][0].ExpirationDate);
            Assert.AreEqual(new DateTime(2001, 1, 1), result[1][1].ExpirationDate);
        }

        [Test]
        public async Task SortedPalletWeightTest()
        {
            Mock<IStorageRepository> mockRepository = new Mock<IStorageRepository>();
            Pallet pallet1 = new Pallet();
            Pallet pallet2 = new Pallet();
            Pallet pallet3 = new Pallet();
            pallet1.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2001, 1, 1), Weight = 60 });
            pallet2.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2001, 1, 1), Weight = 20 });
            pallet2.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2002, 1, 1), Weight = 30 });
            pallet3.Boxes.Add(new Box() { ProductionDate = new DateTime(2000, 1, 1), Weight = 40 });
            pallet3.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2002, 1, 1), Weight = 50 });
            pallet3.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2003, 1, 1), Weight = 60 });
            var includes = new List<Expression<Func<Pallet, object>>>() { x => x.Boxes };
            mockRepository.Setup(m => m.Get(null, null, null, null, It.IsAny<ICollection<Expression<Func<Pallet, object>>>>())).Returns(
                async () => new List<Pallet>() { pallet1, pallet2, pallet3 }.AsQueryable());
            Mock<IMapper> mockmapper = new Mock<IMapper>();
            palletService = new PalletService(mockRepository.Object, mockmapper.Object);

            var result = await palletService.GetSortedPallet();

            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEqual(new List<Pallet>() { pallet2, pallet1 }, result[1]);
            CollectionAssert.AreEqual(new List<Pallet>() { pallet3 }, result[0]);
            Assert.AreEqual(180, result[0][0].Weight);
            Assert.AreEqual(80, result[1][0].Weight);
            Assert.AreEqual(90, result[1][1].Weight);
        }

        [Test]
        public async Task LongLIfePalletTest()
        {
            Mock<IStorageRepository> mockRepository = new Mock<IStorageRepository>();
            Pallet pallet1 = new Pallet() { Width = 1, Height = 1, Deep = 1 };
            Pallet pallet2 = new Pallet();
            Pallet pallet3 = new Pallet();
            Pallet pallet4 = new Pallet();
            Pallet pallet5 = new Pallet() { Width = 10, Height = 5, Deep = 2 };
            pallet1.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2001, 1, 1), Height = 1, Width = 1, Deep = 1 });
            pallet2.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2001, 1, 1), Height = 2, Width = 3, Deep = 5 });
            pallet2.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2002, 1, 1), Height = 3, Width = 5, Deep = 7 });
            pallet3.Boxes.Add(new Box() { ProductionDate = new DateTime(2000, 1, 1) });
            pallet3.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2002, 1, 1) });
            pallet3.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2003, 1, 1) });
            pallet4.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2010, 1, 1) });
            pallet4.Boxes.Add(new Box() { ProductionDate = new DateTime(2000, 1, 1) });
            pallet5.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2010, 1, 1) });
            pallet5.Boxes.Add(new Box() { ExpirationDateSet = new DateTime(2010, 1, 1)});
            var includes = new List<Expression<Func<Pallet, object>>>() { x => x.Boxes };
            mockRepository.Setup(m => m.Get(null, null, null, null, It.IsAny<ICollection<Expression<Func<Pallet, object>>>>())).Returns(
                async () => new List<Pallet>() { pallet1, pallet2, pallet3, pallet4, pallet5 }.AsQueryable());
            Mock<IMapper> mockmapper = new Mock<IMapper>();
            palletService = new PalletService(mockRepository.Object, mockmapper.Object);

            var result = await palletService.GetLongestLifePallets();

            CollectionAssert.AreEqual(new List<Pallet>() { pallet1, pallet5, pallet2 }, result);
            Assert.AreEqual(2, result[0].Volume);
            Assert.AreEqual(100, result[1].Volume);
            Assert.AreEqual(135, result[2].Volume);
        }
    }
}
