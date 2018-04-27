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

        public AuctionAppTest(IUserManager userManager)
        {
            this.um = userManager;
            Run();
        }

        public void Run()
        {
            Console.ReadKey();
        }
    }
}