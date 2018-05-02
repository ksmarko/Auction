using System;

namespace BLL.Infrastructure
{
    public class AuctionException : Exception
    {
        public AuctionException(string message) : base(message) { }

        public AuctionException() : base() { }
    }
}
