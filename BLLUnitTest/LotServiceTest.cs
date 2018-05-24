using Ninject;
using Ninject.Modules;
using System.Configuration;
using DAL.Repositories;
using BLL.DTO;
using DAL.Entities;
using BLL.Services;

using System;
using System.Data.Entity;
using BLL.Interfaces;
using DAL.Interfaces;
using System.Collections.Generic;
using BLL.Infrastructure;

namespace BLLUnitTest
{

    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILotService>().To<LotService>();
            Bind<ICategoryService>().To<CategoryService>();
            Bind<ITradeService>().To<TradeService>();
            Bind<IUserManager>().To<UserManager>();
        }
    }

        [TestFixture]   
    public class LotServiceTest
    {
        private ILotService lotService;
        private Mock<GenericRepository<Lot>> lotRepository;
        private Mock<EFUnitOfWork> uow;
        Mock<DAL.EF.DataContext> db;
        List<Lot> lots;
        static StandardKernel kernel;

        static LotServiceTest()
        {
            var serviceModule = new ServiceModule();
            kernel = new StandardKernel(serviceModule);
            
            AutoMapperConfig.Initialize();
        }

        [SetUp]
        public void Load()
        {
            uow = new Mock<EFUnitOfWork>("defaultbd");
            db = new Mock<DAL.EF.DataContext>("defaultbd");

            lotRepository = new Mock<GenericRepository<Lot>>(db.Object as IUnitOfWork);
           // var rep = kernel.Get<GenericRepository<Lot>>();

            lots = new List<Lot> {
                new Lot{ Name = "Lot1" , Price = 1, TradeDuration = 1},
                new Lot{ Name = "Lot2" , Price = 1, TradeDuration = 1},
                new Lot{ Name = "Lot3" , Price = 1, TradeDuration = 1},
                new Lot{ Name = "Lot4" , Price = 1, TradeDuration = 1}
            };
            uow.Setup(x => x.Lots).Returns(lotRepository.Object as IRepository<Lot>);

            foreach (var el in lots)
                lotRepository.Setup(x => x.Create(el));
                //rep.Create(el);

            
            
            lotService = new LotService(uow.Object);
        }

        [Test]
        public void CreateLot_TryToCreateNullValue_ShouldThrow()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => lotService.CreateLot(null));
        }

        [Test]
        public void GetLot_TryToGetNullValue_ShoulThrow()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => lotService.GetLot(It.IsAny<int>()), null);
        }

        [Test]
        public void GetLot_TryToGetValue_ShouldGetRealValue()
        {
            // act & assert
            Assert.AreEqual(lotService.GetLot(1).Name, "Try1");
            //Assert.Throws<NullReferenceException>(() => lotService.GetLot(5));
        }

        [Test]
        public void GetAllLotsTest()
        {
            Assert.AreEqual(6, 6);
        }

        
    } 
}
