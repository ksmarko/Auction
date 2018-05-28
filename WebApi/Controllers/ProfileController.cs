using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize]
    public class ProfileController : ApiController
    {
        readonly ILotService lotService;
        readonly ITradeService tradeService;
        readonly IUserManager userManager;

        UserDTO CurrentUser
        {
            get => userManager.GetUserByName(User.Identity.Name);
        }

        public ProfileController(ILotService lotService, IUserManager userManager, ITradeService tradeService)
        {
            this.lotService = lotService;
            this.userManager = userManager;
            this.tradeService = tradeService;
        }

        [HttpGet]
        [Route("api/users/current")]
        public string GetUserRole()
        {
            return CurrentUser.Role;
        }

        [HttpGet]
        [Route("api/profile/lots")]
        public IEnumerable<LotModel> GetUserLots()
        {
            return Mapper.Map<IEnumerable<LotDTO>, IEnumerable<LotModel>>(CurrentUser.Lots);
        }

        [HttpGet]
        [Route("api/profile/lots/active")]
        public IEnumerable<LotModel> InPurchase()
        {
            var activeLots = CurrentUser.Lots.Where(x => tradeService.GetTradeByLot(x.Id) != null);

            return Mapper.Map<IEnumerable<LotDTO>, IEnumerable<LotModel>>(activeLots);
        }

        [HttpGet]
        [Route("api/profile/trades/active")]
        public IEnumerable<TradeModel> GetActiveTrades()
        {
            return Mapper.Map<IEnumerable<TradeDTO>, IEnumerable<TradeModel>>(CurrentUser.Trades);
        }

        [HttpGet]
        [Route("api/profile/trades/win")]
        public IEnumerable<TradeModel> GetWinTrades()
        {
            return Mapper.Map<IEnumerable<TradeDTO>, IEnumerable<TradeModel>>(tradeService.GetUserWinTrades(CurrentUser.Id));
        }

        [HttpGet]
        [Route("api/profile/trades/lose")]
        public IEnumerable<TradeModel> GetLoseTrades()
        {
            return Mapper.Map<IEnumerable<TradeDTO>, IEnumerable<TradeModel>>(tradeService.GetUserLoseTrades(CurrentUser.Id));
        }
    }
}
