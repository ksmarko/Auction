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

        //<UserId, UserPrice>
        public IDictionary<double, string> Rates { get; set; }

        public TradeDTO()
        {
            Rates = new Dictionary<double, string>();
        }

        public override string ToString()
        {
            return "Trade for " + Lot.Name;
        }
    }
}
