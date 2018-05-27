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
        IEnumerable<TradeDTO> GetUserLoseTrades(string userId);
        IEnumerable<TradeDTO> GetUserWinTrades(string userId);
        void Dispose();
    }
}
