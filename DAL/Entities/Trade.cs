using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Trade
    {
        [Key]
        public int Id { get; set; }

        public virtual Lot Lot { get; set; }
        public int LotId { get; set; }
        public DateTime StartTrade { get; set; }
        public string LastRateUserId { get; set; }
        public double LastPrice { get; set; }
    }
}
