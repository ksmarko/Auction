using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class TradesController : ApiController
    {
        readonly ITradeService tradeService;
        readonly IUserManager userManager;

        public TradesController(IUserManager userManager, ITradeService tradeService)
        {
            this.userManager = userManager;
            this.tradeService = tradeService;
        }

        [HttpGet]
        [Authorize(Roles = "admin, moderator")]
        public IHttpActionResult StartTrade(LotModel model)
        {
            var user = userManager.GetUserByName(User.Identity.Name);
            var lot = Mapper.Map<LotModel, LotDTO>(model);
            tradeService.StartTrade(lot.Id);

            return Ok("Trade started");
        }


    }
}