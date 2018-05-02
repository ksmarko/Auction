using System;
using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private DataContext db;
        private GenericRepository<Lot> lotRepository;
        private GenericRepository<Trade> tradeRepository;
        private GenericRepository<Category> categoryRepository;
        private UserRepository userRepository;

        public EFUnitOfWork(string connectionString)
        {
            db = new DataContext(connectionString);
        }

        public IRepository<Lot> Lots
        {
            get
            {
                if (lotRepository == null)
                    lotRepository = new GenericRepository<Lot>(db);
                return lotRepository;
            }
        }

        public IRepository<Trade> Trades
        {
            get
            {
                if (tradeRepository == null)
                    tradeRepository = new GenericRepository<Trade>(db);
                return tradeRepository;
            }
        }

        public IRepository<Category> Categories
        {
            get
            {
                if (categoryRepository == null)
                    categoryRepository = new GenericRepository<Category>(db);
                return categoryRepository;
            }
        }

        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
