using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using BLL.Infrastructure;
using BLL.Services;
using DAL.Entities;
using DAL.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using BLL.Exceptions;

namespace BLLUnitTest.Service
{
    [TestFixture]
    public class LotServiceTest
    {
        private ILotService lotService;
        private Mock<IUnitOfWork> uow;
        private Mock<IRepository<Lot>> lotRepository;

        static LotServiceTest()
        {
            try
            {
                Mapper.Initialize(cfg =>
                AutoMapperConfig.Configure(cfg)
                );
            }
            catch { }
        } 

        [SetUp]
        public void Load()
        {
            uow = new Mock<IUnitOfWork>();
            lotRepository = new Mock<IRepository<Lot>>();

            uow.Setup(x => x.Lots).Returns(lotRepository.Object);
            uow.Setup(x => x.Categories.Get(It.IsAny<int>())).Returns(new Category { Name = It.IsAny<string>()});

            lotService = new LotService(uow.Object);
        }

        [Test]
        public void CreateLot_TryToCreateNullValue_ShouldThrowException()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => lotService.CreateLot(null));
        }

        [Test]
        public void Createlot_TryToCreateElementWithNullUser()
        {
            //arrange
            var lot = new LotDTO { Name = It.IsAny<string>(), Price = It.IsAny<double>(), TradeDuration = It.IsAny<int>(), User =null };
            uow.Setup(x => x.Users.Get(It.IsAny<string>())).Returns<User>(null);

            Assert.Throws<ArgumentNullException>(() => lotService.CreateLot(lot));            
        }

        [Test]
        public void CreateLot_TryToCreateLot_ShouldRepositoryCreateOnce()
        {
            //arrange
            var lot = new LotDTO { Name = It.IsAny<string>(), Price = It.IsAny<double>(), TradeDuration = It.IsAny<int>(), User = new UserDTO { Name = It.IsAny<string>() } };
            uow.Setup(x => x.Users.Get(It.IsAny<string>())).Returns(new User { Name = It.IsAny<string>() });

            // act
            lotService.CreateLot(lot);

            //assert
            lotRepository.Verify(x => x.Create(It.IsAny<Lot>()), Times.Once);
        }


        [Test]
        public void GetLot_TryToGetNullValue_ShouldThrowException()
        {
            //arrange
            lotRepository.Setup(x => x.Get(It.IsAny<int>())).Returns<Lot>(null);

            // act & assert
            Assert.IsNull(lotService.GetLot(It.IsAny<int>()));
        }

        [Test]
        public void GetLot_TryToGetValue_ShouldReturnSomeValue()
        {
            //arrange
            var lot = new Lot { Name = It.IsAny<string>(), Price = It.IsAny<double>(), TradeDuration = It.IsAny<int>() };
            uow.Setup(x => x.Lots.Get(It.IsAny<int>())).Returns(lot);

            // act & assert
            Assert.IsNotNull(lotService.GetLot(It.IsAny<int>()));
        }

        [Test]
        public void EditLot_TryToPutInEditNullElement_ShouldThrowException()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => lotService.EditLot(null));
        }

        [Test]
        public void EditLot_TryToEditNullElement_ShouldThrowException()
        {
            //arrange
            var lot = new LotDTO { Name = It.IsAny<string>(), Price = It.IsAny<double>(), TradeDuration = It.IsAny<int>() };
            lotRepository.Setup(x => x.Get(It.IsAny<int>())).Returns<Lot>(null);

            //act & assert
            Assert.Throws<ArgumentNullException>(() => lotService.EditLot(lot));
        }

        [Test]
        public void EditLot_TryToEditVerifiedLot_ShouldThrowException()
        {
            //arrange
            var lot = new LotDTO { Name = It.IsAny<string>(), Price = It.IsAny<double>(), TradeDuration = It.IsAny<int>() };
            lotRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Lot { Name = It.IsAny<string>(), Price = It.IsAny<double>(), TradeDuration = It.IsAny<int>(), IsVerified = true });

            //act & assert
            var ex = Assert.Throws<AuctionException>(() => lotService.EditLot(lot));
            Assert.AreEqual(ex.Message, "You can`t change the information about the lot after the start of the bidding");
        }

        [Test]
        public void EditLot_EditLot_ShoudRepositoryEditOnce()
        {
            //arrange
            var lot = new LotDTO { Name = It.IsAny<string>(), Price = It.IsAny<double>(), TradeDuration = It.IsAny<int>() };
            lotRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Lot { Name = It.IsAny<string>(), Price = It.IsAny<double>(), TradeDuration = It.IsAny<int>()});

            //act
            lotService.EditLot(lot);

            //assert
            lotRepository.Verify(x => x.Update(It.IsAny<Lot>()), Times.Once);
        }

        [Test]
        public void DeleteLot_DeleteNullValue()
        {
            //arrange
            lotRepository.Setup(x => x.Get(It.IsAny<int>())).Returns<Lot>(null);

            //act & assert
            Assert.Throws<ArgumentNullException>(() => lotService.RemoveLot(It.IsAny<int>()));
        }

        [Test]
        public void DeleteLot_DeleteRepositoryShouldCallsOnce()
        {
            //arrange
            lotRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Lot { Name = It.IsAny<string>(), Price = It.IsAny<double>(), TradeDuration = It.IsAny<int>() });

            //act
            lotService.RemoveLot(It.IsAny<int>());

            //assert
            lotRepository.Verify(x => x.Delete(It.IsAny <int>()));
        }
        
        [Test]
        public void VarifyLot_TryToVarifyNullLot_ShouldThrowException()
        {
            //arrange
            lotRepository.Setup(x => x.Get(It.IsAny<int>())).Returns<Lot>(null);

            //act & assert
            Assert.Throws<ArgumentNullException>(() => lotService.VerifyLot(It.IsAny<int>()));
        }

        [Test]
        public void VarifyLot_TryToVirifySomeLot_ShouldCallOnce()
        {
            //arrange
            lotRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Lot { Name = It.IsAny<string>(), Price = It.IsAny<double>(), TradeDuration = It.IsAny<int>() });

            //act 
            lotService.VerifyLot(It.IsAny<int>());

            //assert
            lotRepository.Verify(x => x.Update(It.IsAny<Lot>()), Times.Once);
        }

        [Test]
        public void ChangeLotCategory_TryToChangeWithNullCategory_ShouldThrowException()
        {
            //arrange
            uow.Setup(x => x.Categories.Get(It.IsAny<int>())).Returns<Category>(null);

            //act & assert
            Assert.Throws<ArgumentNullException>(() => lotService.ChangeLotCategory(It.IsAny<int>(), It.IsAny<int>()));
        }

        [Test]
        public void ChangeLotCategory_TryToChangeWithNullLot_ShouldThrowException()
        {
            //arrange
            lotRepository.Setup(x => x.Get(It.IsAny<int>())).Returns<Lot>(null);

            //act & assert
            Assert.Throws<ArgumentNullException>(() => lotService.ChangeLotCategory(It.IsAny<int>(), It.IsAny<int>()));
        }

        [Test]
        public void ChangeLotCategory_TryToChange_ShouldCallsOnce()
        {
            //arrange
            lotRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Lot { Name = It.IsAny<string>(), Price = It.IsAny<double>(), TradeDuration = It.IsAny<int>() });

            //act
            lotService.ChangeLotCategory(It.IsAny<int>(), It.IsAny<int>());

            //assert
            lotRepository.Verify(x => x.Update(It.IsAny<Lot>()), Times.Once);
        }

        [Test]
        public void GetAllLots_TryToGetSomeList_ShouldRepositoryCallOnce_ShouldReturnNotNullList()
        {
            //arrange
            lotRepository.Setup(x => x.GetAll()).Returns(new List<Lot>() { });

            //act & assert
            Assert.IsNotNull(lotService.GetAllLots());
            lotRepository.Verify(x => x.GetAll());
        }
    } 
}
    