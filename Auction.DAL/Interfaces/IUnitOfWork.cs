using System;
using Auction.DAL.Entities;

namespace Auction.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Lot> Lots { get; }
        IRepository<Trade> Trades { get; }
        IRepository<Category> Categories { get; }
        IUserRepository Users { get; }
        void Save();
    }
}
