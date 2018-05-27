using BLL.DTO;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface ITradeService
    {
        void StartTrade(int lotId);
        void Rate(int tradeId, string userId, double price);
        IEnumerable<TradeDTO> GetAllTrades();
        TradeDTO GetTrade(int id);        
        TradeDTO GetTradeByLot(int id);
        IEnumerable<TradeDTO> GetUserLoseTrades(string id);
        IEnumerable<TradeDTO> GetUserWinTrades(string id);
        IEnumerable<TradeDTO> GetUserActiveTrades(string id);
        void Dispose();
    }
}
