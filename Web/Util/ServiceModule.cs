﻿using BLL.Interfaces;
using BLL.Services;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Util
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITradeService>().To<TradeService>();
            Bind<ILotService>().To<LotService>();
            Bind<IUserManager>().To<UserManager>();
        }
    }
}