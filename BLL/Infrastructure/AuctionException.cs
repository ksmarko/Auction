using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public class AuctionException : Exception
    {
        public AuctionException(string message) : base(message) { }
        public AuctionException() : base() { }
    }
}
