using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Identity.Entities;
using Auction.DAL.Identity.Interfaces;

namespace Auction.DAL.Identity.Repositories
{
    public class ClientManager : IClientManager
    {
        public DataContext Database { get; set; }
        public ClientManager(DataContext db)
        {
            Database = db;
        }

        public void Create(User item)
        {
            Database.UserProfiles.Add(item);
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
