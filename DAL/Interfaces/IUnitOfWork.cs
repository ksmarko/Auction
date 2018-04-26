using System;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;

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
