using System;

namespace BLL.Exceptions
{
    public class AuctionException : Exception
    {
        public AuctionException(string message) : base(message) { }

        public AuctionException() : base() { }
    }
}
