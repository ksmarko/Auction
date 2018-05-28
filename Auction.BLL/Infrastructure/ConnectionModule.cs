using Auction.DAL.Identity.Interfaces;
using Auction.DAL.Identity.Repositories;
using Auction.DAL.Interfaces;
using Auction.DAL.Repositories;
using Ninject;
using Ninject.Modules;

namespace Auction.BLL.Infrastructure
{
    public class ConnectionModule : NinjectModule
    {
        private string connectionString;

        public ConnectionModule(string connection)
        {
            connectionString = connection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(connectionString);
            Bind<IUnitOfWorkIdentity>().To<IdentityUnitOfWork>().WithConstructorArgument(connectionString);
        }
    }
}
