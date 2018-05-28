using AutoMapper;
using Auction.BLL.DTO;
using Auction.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Auction.WEB.Models;

namespace Auction.WEB.Controllers
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

        [HttpPut]
        [Route("api/trades/start/{id}")]
        [Authorize(Roles = "admin, moderator")]
        public IHttpActionResult StartTrade(int id)
        {
            tradeService.StartTrade(id);

            return Ok("Trade started");
        }

        [HttpPut]
        [Route("api/trades/rate")]
        public void Rate(RateModel model)
        {
            var user = userManager.GetUserByName(User.Identity.Name).Id;
            tradeService.Rate(model.TradeId, user, model.Price);
        }

        [HttpGet]
        public IEnumerable<TradeModel> GetTrades()
        {
            return Mapper.Map<IEnumerable<TradeDTO>, IEnumerable<TradeModel>>(tradeService.GetAllTrades());
        }
    }
}