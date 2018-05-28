using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using BLL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    /// <summary>
    /// Service for work with trades
    /// </summary>
    public class TradeService : ITradeService
    {
        /// <summary>
        /// Represents domain database
        /// </summary>
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Creates service
        /// </summary>
        /// <param name="uow">UnitOfWork</param>
        public TradeService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        /// <summary>
        /// Stert trade
        /// </summary>
        /// <param name="lotId">Lot Id</param>
        /// <exception cref="ArgumentNullException">When lot not found</exception>
        /// <exception cref="AuctionException">When trde with this lot has alredy began or lot not verify</exception>
        public void StartTrade(int lotId)
        {
            Lot lot = Database.Lots.Get(lotId);

            if (lot == null)
                throw new ArgumentNullException();

            if (GetTradeByLot(lotId) != null)
                throw new AuctionException($"Trade for lot: {lot.Name} has already began");

            if(!lot.IsVerified)
                throw new AuctionException("Lot is not verified");

            Database.Trades.Create(new Trade
            {
                Lot = lot,
                TradeStart = DateTime.Now,
                TradeEnd = DateTime.Now.AddDays(lot.TradeDuration)
            });

            Database.Save();
        }

        /// <summary>
        /// Rate lot
        /// </summary>
        /// <param name="tradeId">Trade Id</param>
        /// <param name="userId">New User Id</param>
        /// <param name="price">New Price</param>
        /// <exception cref="ArgumentNullException">When trade nit found</exception>
        /// <exception cref="AuctionException">When owner try to rate his lot or when trade is over or when new price smaller then old</exception>
        public void Rate(int tradeId, string userId, double price)
        {
            Trade trade = Database.Trades.Get(tradeId);
            User user = Database.Users.Get(userId);

            if (trade == null || user == null)
                throw new ArgumentNullException();

            if (trade.Lot.User.Id == user.Id)
                throw new AuctionException("This is your lot");

            if (DateTime.Now.CompareTo(trade.TradeEnd) >= 0)
                throw new AuctionException("This trade is over");

            bool isNew = true;

            foreach (var el in user.Trades)
                if (el.Id == trade.Id)
                    isNew = false;

            if (trade.LastPrice < price)
            {
                trade.LastPrice = price;
                trade.LastRateUserId = userId;
                if (isNew)
                    user.Trades.Add(trade);
            }
            else
                throw new AuctionException($"Your price should be greater than: {trade.LastPrice}");
            
            Database.Users.Update(user);
            Database.Trades.Update(trade);
            Database.Save();
        }
        
        /// <summary>
        /// Get ol trade
        /// </summary>
        /// <returns>Return list of trades</returns>
        public IEnumerable<TradeDTO> GetAllTrades()
        {
            return Mapper.Map<IEnumerable<Trade>, List<TradeDTO>>(Database.Trades.GetAll());
        }

        /// <summary>
        /// Get trade by Id
        /// </summary>
        /// <param name="id">Trade Id</param>
        /// <returns></returns>
        public TradeDTO GetTrade(int id)
        {
            return Mapper.Map<Trade, TradeDTO>(Database.Trades.Get(id));
        }

        /// <summary>
        /// Get trade by lot
        /// </summary>
        /// <param name="id">Lot Id</param>
        public TradeDTO GetTradeByLot(int id)
        {
            return Mapper.Map<Trade, TradeDTO>(Database.Trades.Find(x => x.LotId == id).FirstOrDefault());
        }

        /// <summary>
        /// Get all trade that user has win
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of trade</returns>
        public IEnumerable<TradeDTO> GetUserLoseTrades(string userId)
        {
            var user = Database.Users.Get(userId);

            if (user == null)
                throw new ArgumentNullException();

            var list = user.Trades.Where(x => DateTime.Now.CompareTo(x.TradeEnd) >= 0 && x.LastRateUserId != user.Id);

            return Mapper.Map<IEnumerable<Trade>, List<TradeDTO>>(list);
        }

        /// <summary>
        /// Get all trade that user has lose
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of trade</returns>
        public IEnumerable<TradeDTO> GetUserWinTrades(string userId)
        {
            var user = Database.Users.Get(userId);

            if (user == null)
                throw new ArgumentNullException();

            var list = user.Trades.Where(x => DateTime.Now.CompareTo(x.TradeEnd) >= 0 && x.LastRateUserId == user.Id);

            return Mapper.Map<IEnumerable<Trade>, List<TradeDTO>>(list);
        }
    }
}
