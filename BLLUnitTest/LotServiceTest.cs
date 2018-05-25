using Ninject;
using Ninject.Modules;
using System.Configuration;
using DAL.Repositories;
using BLL.DTO;
using DAL.Entities;
using BLL.Services;
using Moq;
using NSubstitute;
//using Ploeh.AutoFixture;
using NUnit.Framework;
using System;
using System.Data.Entity;
using BLL.Interfaces;
using DAL.Interfaces;
using System.Collections.Generic;
using BLL.Infrastructure;
using AutoMapper;

namespace BLLUnitTest
{
    [TestFixture]
    public class LotServiceTest
    {
        private ILotService lotService;
        private Mock<IUnitOfWork> uow;
        private Mock<IRepository<Lot>> lotRepository;

        static LotServiceTest()
        {
            Mapper.Initialize(cfg =>
            BLL.Infrastructure.AutoMapperConfig.Configure(cfg)
            );
        } 

        [SetUp]
        public void Load()
        {
            uow = new Mock<IUnitOfWork>();
            lotRepository = new Mock<IRepository<Lot>>();

            uow.Setup(x => x.Lots).Returns(lotRepository.Object);
            uow.Setup(x => x.Categories.Get(It.IsAny<int>())).Returns(It.IsAny<Category>());

            lotService = new LotService(uow.Object);
        }

        [Test]
        public void CreateLot_TryToCreateNullValue_ShouldThrow()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => lotService.CreateLot(null));
        }


        [Test]
        public void CreateLot_TryToCreateLot_ShouldRepositoryCreateOnce()
        {
            var lot = new LotDTO { Name = It.IsAny<string>(), Price = It.IsAny<double>(), TradeDuration = It.IsAny<int>()};
            
            // act
            lotService.CreateLot(lot);

            //assert
            lotRepository.Verify(x => x.Create(It.IsAny<Lot>()), Times.Once);
        }


        [Test]
        public void GetLot_TryToGetNullValue_ShouldThrow()
        {
            // act & assert
            Assert.IsNull(lotService.GetLot(It.IsAny<int>()));
        }

        [Test]
        public void GetLot_TryToGetValue_ShouldReturnSomeValue()
        {
            var lot = new Lot { Name = It.IsAny<string>(), Price = It.IsAny<double>(), TradeDuration = It.IsAny<int>() };

            uow.Setup(x => x.Lots.Get(It.IsAny<int>())).Returns(lot);

            Assert.IsNotNull(lotService.GetLot(It.IsAny<int>()));
        }

        
    } 
}
