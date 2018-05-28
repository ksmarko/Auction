using Auction.DAL.Entities;
using Auction.DAL.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.DAL.Identity.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create(User item);
    }
}
