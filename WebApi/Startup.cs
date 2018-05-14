using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebApi.Startup))]

namespace WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
