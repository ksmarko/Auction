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

        //<UserId, UserPrice>
        public virtual IDictionary<int, double> Rates { get; set; }

        public Trade()
        {
            Rates = new Dictionary<int, double>();
        }
    }
}
