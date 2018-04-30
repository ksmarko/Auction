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
            //ls.CreateLot(new LotDTO { TradeDuration = new DateTime(2018, 5, 01, 19, 45, 00), Name = "MyLot1"});
            //ls.EditLot(new LotDTO { Name = "MyEditLot4", Id = 1, TradeDuration = new DateTime(2018, 4, 28, 19, 45, 00) });
            //ls.RemoveLot(1);

            //cs.CreateCategory(new CategoryDTO() { Name = "MyCategory"});
            cs.RemoveCategory(2);
            //cs.EditCategory(new CategoryDTO() { Name = "MyEditCategory", Id = 1});

            //ls.AddLotToCategory(1, 2);
            //ls.RemoveLotFromCategory(1, 1);
            //ls.RemoveLotFromCategory(1, 2);

            //foreach (var el in cs.GetAllCategory())
                //foreach(var a in el.Lots)
                    //Console.WriteLine(el + "\t" + a);
            //Console.WriteLine($"Category: {cs.GetCategory(8)}");
            //foreach (var el in cs.GetCategory(8).Lots)
            //Console.WriteLine($"Lot: {el}");

            //foreach (var el in ls.GetAll())
            //    Console.WriteLine(el);

            //ts.StartTrade(1);
            //ts.Rate(1, "f0e55c71-2e6c-41df-bea1-1ff5dc102f59", 15.0);
            //Сonsole.WriteLine("Start:");

            //foreach (var el in ts.GetTrade(1).Rates)
                //Console.WriteLine($"User: {um.GetUserById(el.Value).Name}");
                
            //Console.WriteLine("Start:");
            //Console.WriteLine(ts.GetTrade(1).Lot.WinUser.Name);
            //Console.WriteLine(":End");

            //Console.ReadKey();
        }
    }
}