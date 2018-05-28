using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Auction.WEB.Startup))]

namespace Auction.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
