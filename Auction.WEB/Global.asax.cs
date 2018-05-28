using AutoMapper;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Auction.WEB.App_Start;

namespace Auction.WEB
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));

            Mapper.Initialize(cfg =>
            {
                BLL.Infrastructure.AutoMapperConfig.Configure(cfg);
                AutoMapperConfig.Configure(cfg);
            });
        }
    }
}
