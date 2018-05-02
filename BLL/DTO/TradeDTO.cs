using System;

namespace BLL.DTO
{
    public class TradeDTO
    {
        public int Id { get; set; }
        public LotDTO Lot { get; set; }
        public DateTime TradeStart { get; set; }
        public DateTime TradeEnd { get; set; }
        public string LastRateUserId { get; set; }
        public double LastPrice { get; set; }

        public override string ToString()
        {
            return "Trade for " + Lot.Name;
        }
    }
}
