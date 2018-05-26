using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class TradeModel
    {
        public int Id { get; set; }
        public LotModel Lot { get; set; }
        public int DaysLeft { get; set; }
        public string TradeEnd { get; set; }
        public double LastPrice { get; set; }
    }
}