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
        readonly ICategoryService cs;
        readonly ITradeService ts;

        public AuctionAppTest(IUserManager userManager, ILotService lotService, ICategoryService categoryService, ITradeService tradeService)
        {
            this.um = userManager;
            this.ls = lotService;
            this.cs = categoryService;
            this.ts = tradeService;
            Run();
        }

        public void Run()
        {

        }
    }
}