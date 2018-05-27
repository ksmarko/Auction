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
    public class TradeServiceTest
    {
        static TradeServiceTest()
        {
            try
            {
                Mapper.Initialize(cfg =>
                AutoMapperConfig.Configure(cfg)
                );
            }
            catch { }
        }

        private ITradeService tradeService;
        private Mock<IUnitOfWork> uow;
        private Mock<IRepository<Trade>> tradeRepository;
        
        [SetUp]
        public void Load()
        {
            uow = new Mock<IUnitOfWork>();
            tradeRepository = new Mock<IRepository<Trade>>();

            uow.Setup(x => x.Trades).Returns(tradeRepository.Object);
            uow.Setup(x => x.Lots.Get(It.IsAny<int>())).Returns(new Lot {Name = It.IsAny<string>(), User = It.IsAny<User>(), TradeDuration = It.IsAny<int>(), Price = It.IsAny<double>(), IsVerified = true});
            uow.Setup(x => x.Users.Get(It.IsAny<string>())).Returns(new User { Id = "defId" });

            tradeService = new TradeService(uow.Object);
        }

        [Test]
        public void StartTrade_TryToStartWithNullLot_ShouldThrowException()
        {
            //arrange
            uow.Setup(x => x.Lots.Get(It.IsAny<int>())).Returns<Lot>(null);

            //act & arrange
            Assert.Throws<ArgumentNullException>(() => tradeService.StartTrade(It.IsAny<int>()));
        }

        [Test]
        public void StartTrade_TryToStartTradeThatAlreadyBegun_ShouldThrowException()
        {
            //arrange
            tradeRepository.Setup(x => x.Find(It.IsAny<Func<Trade, bool>>())).Returns(new List<Trade> { new Trade { LastPrice = It.IsAny<double>() } });
            var lot = uow.Object.Lots.Get(It.IsAny<int>());

            //act & arrange
            var ex = Assert.Throws<AuctionException>(() => tradeService.StartTrade(lot.Id));
            Assert.AreEqual(ex.Message, $"Trade for lot: {lot.Name} has already began");
        }

        [Test]
        public void StartTrade_TryToStartTradeWithNotVarifyLot_ShouldThrowException()
        {
            //arrange
            uow.Setup(x => x.Lots.Get(It.IsAny<int>())).Returns(new Lot { Name = It.IsAny<string>(), User = It.IsAny<User>(), TradeDuration = It.IsAny<int>(), Price = It.IsAny<double>(), IsVerified = false});
            var lot = uow.Object.Lots.Get(It.IsAny<int>());
            
            //act & assert
            var ex = Assert.Throws<AuctionException>(() => tradeService.StartTrade(lot.Id));
            Assert.AreEqual(ex.Message, "Lot is not verified");
        }

        [Test]
        public void StartTrade_TryToStartTrade_TradeRepositiryShouldCallsOnce()
        {
            //arrange
            var lot = uow.Object.Lots.Get(It.IsAny<int>());

            //act
            tradeService.StartTrade(lot.Id);

            //assert
            tradeRepository.Verify(x => x.Create(It.IsAny<Trade>()), Times.Once);
        }

        [Test]
        public void Rate_TryToRateWithNullUser_ShouldThrowException()
        {
            //arrange
            uow.Setup(x => x.Users.Get(It.IsAny<string>())).Returns<User>(null);

            //act & assert
            Assert.Throws<ArgumentNullException>(() => tradeService.Rate(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<double>()));
        }

        [Test]
        public void Rate_TryToRateWithNullTrade_ShouldThrowException()
        {
            //arrange
            uow.Setup(x => x.Trades.Get(It.IsAny<int>())).Returns<Lot>(null);

            //act & assert
            Assert.Throws<ArgumentNullException>(() => tradeService.Rate(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<double>()));
        }

        [Test]
        public void Rate_TryToRateOwnLot_ShouldThrowExcesption()
        {
            //arrange
            var lots = new List<Lot> { };
            var user = new User { Name = It.IsAny<string>(), Lots = lots };
            lots.Add(new Lot { Name = It.IsAny<string>(), User = user });
            var trade = new Trade { Lot = lots[0]};
            uow.Setup(x => x.Trades.Get(It.IsAny<int>())).Returns(trade);
            uow.Setup(x => x.Users.Get(It.IsAny<string>())).Returns(user);

            //act & arrange
            var ex =Assert.Throws<AuctionException>(() => tradeService.Rate(trade.Id, user.Id, It.IsAny<double>()));
            Assert.AreEqual("This is your lot", ex.Message);
        }

        [Test]
        public void Rate_TryToRateEndTraide_ShouldThrowException()
        {
            //arrange
            var trade = new Trade { Lot = new Lot { User = new User { } }, TradeEnd = It.Is<DateTime>(x => x.CompareTo(DateTime.Now) > 0) };
            tradeRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(trade);

            //act & assert
            var ex = Assert.Throws<AuctionException>(() => tradeService.Rate(trade.Id, It.IsAny<string>(), It.IsAny<double>()));
            Assert.AreEqual(ex.Message, "This trade is over");

        }

        [Test]
        public void Rate_TryToRateWithSmolarPrise_ShouldThrowException()
        {
            //arrange
            var trade = new Trade { Lot = new Lot { User = new User { } }, TradeEnd = DateTime.Now.AddDays(+3), LastPrice = It.IsAny<double>() };
            tradeRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(trade);

            //act & assert
            var ex = Assert.Throws<AuctionException>(() => tradeService.Rate(trade.Id, It.IsAny<string>(), It.Is<double>(x => trade.LastPrice > x)));
            Assert.AreEqual(ex.Message, $"Your price should be greater than: {trade.LastPrice}");
        }

        [Test]
        public void Rate_TryToRate_RepositoryShouldCallOnce()
        {
            //arrange
            var trade = new Trade { Lot = new Lot { User = new User { } }, TradeEnd = DateTime.Now.AddDays(+3), LastPrice =  It.IsAny<double>()};
            tradeRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(trade);

            //act
            tradeService.Rate(trade.Id, It.IsAny<string>(), trade.LastPrice +1);

            //assert
            tradeRepository.Verify(x => x.Update(It.IsAny<Trade>()), Times.Once);
        }

        [Test]
        public void GetUserWinTrades_TryToGetNonExistentUserTradeFromAll_ShouldThrowException()
        {
            //arrange
            uow.Setup(x => x.Users.Get(It.IsAny<string>())).Returns<User>(null);
            

            //act & assert
            Assert.Throws<ArgumentNullException>(() => tradeService.GetUserWinTrades(It.IsAny<string>()));
        }

        [Test]
        public void GetUserLoseTrade_TryToGetNonExistentUserTradeFromAll_ShouldThrowException()
        {
            //arrange
            uow.Setup(x => x.Users.Get(It.IsAny<string>())).Returns<User>(null);
            
            //act & assert
            Assert.Throws<ArgumentNullException>(() => tradeService.GetUserLoseTrades(It.IsAny<string>()));
        }

        [Test]
        public void GetUserWinTrade_TryToGetActiveTrades()
        {
            //arrange
            var user = new User { Name = It.IsAny<string>()};
            var allTrades = new List<Trade> { new Trade {TradeEnd = DateTime.Now.AddDays(-1), LastRateUserId = It.Is<string>( x => x == user.Id)},
            new Trade { TradeEnd = DateTime.Now.AddDays(3)},
            new Trade { TradeEnd = DateTime.Now.AddDays(5)}};
            user.Trades = allTrades;

            uow.Setup(x => x.Users.Get(It.IsAny<string>())).Returns(user);

            //act
            List<TradeDTO> list = tradeService.GetUserWinTrades(It.IsAny<string>()) as List<TradeDTO>;

            //assert
            Assert.AreEqual(list.Count, 1);
        }

        [Test]
        public void GetUserLoseTrade_TryToGetActiveTrades()
        {
            //arrange
            var user = new User { Id = "winId", Name = It.IsAny<string>() };
            var allTrades = new List<Trade> { new Trade {TradeEnd = DateTime.Now.AddDays(-1), LastRateUserId = It.Is<string>(x => x != user.Id)},
            new Trade { TradeEnd = DateTime.Now.AddDays(3)},
            new Trade { TradeEnd = DateTime.Now.AddDays(5)}};
            user.Trades = allTrades;

            uow.Setup(x => x.Users.Get(It.IsAny<string>())).Returns(user);

            //act
            List<TradeDTO> list = tradeService.GetUserLoseTrades(It.IsAny<string>()) as List<TradeDTO>;

            //assert
            Assert.AreEqual(list.Count, 1);
        }
    }
}
