﻿using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class TradeService : ITradeService
    {
        IUnitOfWork Database { get; set; }

        public TradeService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

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
            
            foreach (var el in user.Lots)
                if (el.Id == trade.Lot.Id)
                    isNew = false;

            if (trade.LastPrice < price)
            {
                trade.LastPrice = price;
                trade.LastRateUserId = userId;
                if (isNew)
                    user.Lots.Add(trade.Lot);
            }
            else
                throw new AuctionException($"Your price should be greater than: {trade.LastPrice}");

            Database.Lots.Update(trade.Lot);
            Database.Users.Update(user);
            Database.Save();
        }

        public IEnumerable<TradeDTO> GetAllTrades()
        {
            return Mapper.Map<IEnumerable<Trade>, List<TradeDTO>>(Database.Trades.GetAll());
        }

        public TradeDTO GetTrade(int id)
        {
            return Mapper.Map<Trade, TradeDTO>(Database.Trades.Get(id));
        }

        public TradeDTO GetTradeByLot(int id)
        {
            return Mapper.Map<Trade, TradeDTO>(Database.Trades.Find(x => x.LotId == id).FirstOrDefault());
        }

    }
}
