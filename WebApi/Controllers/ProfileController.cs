using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize]
    public class ProfileController : ApiController
    {
        readonly ILotService lotService;
        readonly IUserManager userManager;

        public ProfileController(ILotService lotService, IUserManager userManager)
        {
            this.lotService = lotService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("api/profile/lots")]
        public IEnumerable<LotModel> GetUserLots()
        {
            var user = userManager.GetUsers().Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            var lots = Mapper.Map<IEnumerable<LotDTO>, IEnumerable<LotModel>>(user.Lots);

            return lots;
        }
    }
}
