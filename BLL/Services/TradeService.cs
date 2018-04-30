using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using BLL.Infrastructure;
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

        public void StartTrade(int lotId)
        {
            Lot workLot = Database.Lots.Find(x => x.Id == lotId).FirstOrDefault();
            if (workLot == null)
                throw new ArgumentNullException();

            if (GetTradeByLot(lotId) != null)
                throw new AuctionException($"Trade for lot: {workLot.Name} has allredy began");
             if((workLot.TradeDuration - DateTime.Now).Days < 0)
                //theoretically some message for user
                throw new AuctionException($"You can`t start trade, as user {workLot.User.Name} wont trade end in past");
            Database.Trades.Create(new Trade{ Lot = workLot, LotId = workLot.Id, StartTrade = DateTime.Now });
            Database.Save();
        }

        public void Rate(int tradeId, string userId, double price)
        {
            
            Trade workTrade = Database.Trades.Find(x => x.Id == tradeId).FirstOrDefault();
            User workUser = Database.Users.Find(x => x.Id == userId).FirstOrDefault();
          
            if (workTrade == null || workUser == null)
                throw new ArgumentNullException();
            if (DateTime.Now.CompareTo(workTrade.Lot.TradeDuration) >= 0)
                throw new AuctionException("This trade end");

            bool isNew = true;
            
            foreach (var el in workUser.Lots)
                if (el.Id == workTrade.Lot.Id)
                    isNew = false;

            if (workTrade.LastPrice < price)
            {
                workTrade.LastPrice = price;
                workTrade.LastRateUserId = userId;
                if (isNew)
                    workUser.Lots.Add(workTrade.Lot);
            }
            else
                throw new AuctionException($"Your price must be begger that last: {workTrade.LastPrice}");
            Database.Lots.Update(workTrade.Lot);
            Database.Users.Update(workUser);
            Database.Save();
        }

        //theoretically this function mast work automatically
        

        public IEnumerable<TradeDTO> GetAllTrade()
        {
            return Mapper.Map<IEnumerable<Trade>, List<TradeDTO>>(Database.Trades.GetAll());
        }

        public TradeDTO GetTrade(int tradeId)
        {
            return Mapper.Map<Trade, TradeDTO>(Database.Trades.Get(tradeId));
        }

        public TradeDTO GetTradeByLot(int lotId)
        {
            return Mapper.Map<Trade, TradeDTO>(Database.Trades.Find(x => x.LotId == lotId).FirstOrDefault());
        }

        //public IEnumerable<UserDTO> GetUserInRate(int tradeId)
        //{
        //    Trade workTrade = Database.Trades.Find(x => x.Id == tradeId).FirstOrDefault();
        //    var userDTOList = new List<UserDTO>();

        //    foreach (var el in workTrade.Rates)
        //        userDTOList.Add(Mapper.Map<User, UserDTO>(Database.Users.Get(el.Value)));

        //    return userDTOList;
        //}
    }
}
