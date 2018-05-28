using System;
using System.ComponentModel.DataAnnotations;

namespace Auction.DAL.Entities
{
    public class Trade
    {
        [Key]
        public int Id { get; set; }

        public virtual Lot Lot { get; set; }
        public int LotId { get; set; }

        public DateTime TradeStart { get; set; }

        public DateTime TradeEnd { get; set; }

        public string LastRateUserId { get; set; }

        public double LastPrice { get; set; }
    }
}
