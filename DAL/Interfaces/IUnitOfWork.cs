using System;
using DAL.Entities;

namespace DAL.Interfaces
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
