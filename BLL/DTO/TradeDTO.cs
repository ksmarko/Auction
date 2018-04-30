using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class TradeDTO
    {
        public int Id { get; set; }
        public LotDTO Lot { get; set; }
        public DateTime StartTrade { get; set; }
        public UserDTO WinUser { get; set; }
        public string LastRateUserId { get; set; }
        public double LastPrice { get; set; }

        public override string ToString()
        {
            return "Trade for " + Lot.Name;
        }
    }
}
