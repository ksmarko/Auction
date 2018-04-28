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
        void StartTrade(LotDTO entity);
        void Rate(TradeDTO lot, UserDTO user, double price);
        void EndTrade(TradeDTO entity);
        
        void Dispose();
    }
}
