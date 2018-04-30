using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ITradeService
    {
        void StartTrade(int lotId);
        void Rate(int lotId, string userId, double price);
        IEnumerable<TradeDTO> GetAllTrade();
        TradeDTO GetTrade(int tradeId);
        //IEnumerable<UserDTO> GetUserInRate(int tradeId);
        TradeDTO GetTradeByLot(int lotId);
        void Dispose();
    }
}
