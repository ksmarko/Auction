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
            ls.CreateLot(new LotDTO { TradeDuration = new DateTime(2018, 4, 28, 19, 45, 00), Name = "MuLot"});
            Console.ReadKey();
        }
    }
}