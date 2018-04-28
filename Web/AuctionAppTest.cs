using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    class AuctionAppTest
    {
        readonly IUserManager um;
        readonly ILotService ls;

        public AuctionAppTest(IUserManager userManager, ILotService lotService)
        {
            this.um = userManager;
            this.ls = lotService;
            Run();
        }

        public void Run()
        {
            var lot = new LotDTO() { Name = "MyLot", Price = 800 };
            ls.CreateLot(lot);
            
            //Console.ReadKey();
        }
    }
}