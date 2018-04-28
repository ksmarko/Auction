using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void StartTrade(LotDTO entity)
        {
            Lot workLot = Mapper.Map<LotDTO, Lot>(entity);
            if (workLot == null)
                throw new ArgumentNullException();

            if (workLot.StartTrade != null)
                throw new Exception($"Trade for lot: {workLot.Name} has allredy began");
             if((workLot.TradeDuration - DateTime.Now).Days < 0)
                //theoretically some message for user
                throw new Exception($"You can`t start trade, as user {workLot.User.Name} wont trade end in past");
            workLot.StartTrade = DateTime.Now;
            Database.Lots.Update(workLot);
            Database.Trades.Create(new Trade(){ Lot = workLot, LotId = workLot.Id });
            Database.Save();
        }

        public void Rate(TradeDTO trade, UserDTO user, double price)
        {
            Trade workTrade = Mapper.Map<TradeDTO, Trade>(trade);
            User workUser = Mapper.Map<UserDTO, User>(user);
            bool isNew = true;
            bool isbigger = true;

            if (workTrade == null || workUser == null)
                throw new ArgumentNullException();

            if ((DateTime.Now - workTrade.Lot.TradeDuration).Days < 0)
                throw new Exception("Sory trade is over");
            if ((DateTime.Now - workTrade.Lot.TradeDuration).Days == 0 && (DateTime.Now - workTrade.Lot.TradeDuration).Hours < 0)
                throw new Exception("Sory trade is over");
            foreach (var el in workTrade.Rates)
                if (el.Key > price)
                    isbigger = false;

            foreach (var el in workUser.Lots)
                if (el.Id == workTrade.LotId)
                    isNew = false;

           
            if (isbigger)
            {
                workTrade.Rates.Add(price, workUser.Id);
                workTrade.Lot.Price = price;
                if (isNew)
                    workUser.Lots.Add(workTrade.Lot);

            }
            else
                throw new Exception("Your price coudn`t be less than last");

            Database.Lots.Update(workTrade.Lot);
            Database.Trades.Update(workTrade);
            Database.Save();
        }

        //theoretically this function mast work automatically
        public void EndTrade(TradeDTO entity)
        {
            Trade workTrade = Mapper.Map<TradeDTO, Trade>(entity);
            double winPrice = 0;
            string winId = "";

            foreach (var el in workTrade.Rates)
                if (el.Key > winPrice)
                {
                    winId = el.Value;
                    winPrice = el.Key;
                }
            workTrade.Lot.WinUserID = winId;
            workTrade.Lot.WinUser = Database.Users.Get(winId);
        }
    }
}
