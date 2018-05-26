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
        IEnumerable<TradeDTO> GetUserLoseTrades(UserDTO userDTO);
        IEnumerable<TradeDTO> GetUserWinTrades(UserDTO userDTO);
        IEnumerable<TradeDTO> GetUserActiveTrades(UserDTO userDTO);
        void Dispose();
    }
}
