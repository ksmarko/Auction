using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using WebApi.Models;
using System.Web.Http;
using AutoMapper;
using BLL.DTO;

namespace WebApi.Controllers
{
    public class LotsController : ApiController
    {
        readonly ILotService lotService;
        readonly IUserManager userManager;

        public LotsController(ILotService lotService, IUserManager userManager)
        {
            this.lotService = lotService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("api/lots/create")]
        public IHttpActionResult AddLot(LotModel model)
        {
            var user = userManager.GetUsers().First();
            var lot = Mapper.Map<LotModel, LotDTO>(model);
            lot.User = user;
            lotService.CreateLot(lot);

            return Ok("Lot created");
        }

        [HttpPost]
        [Route("api/lots/edit")]
        public IHttpActionResult EditLot(LotModel model)
        {
            lotService.EditLot(Mapper.Map<LotModel, LotDTO>(model));
            var lot = lotService.GetLot(model.Id);

            return Ok($"Lot {lot.Name} edited\nId = {lot.Id}");
        }

        [HttpDelete]
        [Route("api/lots/remove/{id}")]
        public IHttpActionResult DeleteLot(int id)
        {
            var lot = lotService.GetLot(id);
            lotService.RemoveLot(id);

            return Ok($"Lot {lot.Name} removed\nId = {lot.Id}");
        }

        [HttpGet]
        [Route("api/lots")]
        public IEnumerable<LotModel> GetAllLots()
        {
            var lots = lotService.GetAllLots();

            return Mapper.Map<IEnumerable<LotDTO>, IEnumerable<LotModel>>(lots);
        }
    }
}
