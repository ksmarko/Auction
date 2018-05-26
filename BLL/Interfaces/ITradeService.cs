using BLL.DTO;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface ITradeService
    {
        void StartTrade(int lotId);
        void Rate(int lotId, string userId, double price);
        IEnumerable<TradeDTO> GetAllTrades();
        TradeDTO GetTrade(int id);        
        TradeDTO GetTradeByLot(int id);
        IEnumerable<TradeDTO> GetUserLoseTradess(UserDTO userDTO);
        IEnumerable<TradeDTO> GetAllUserTrades(UserDTO userDTO);
        IEnumerable<TradeDTO> GetUserWinTrades(UserDTO userDTO);
        void Dispose();
    }
}
