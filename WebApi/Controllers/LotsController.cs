﻿using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using WebApi.Models;
using System.Web.Http;
using AutoMapper;
using BLL.DTO;
using System.IO;

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
        [Authorize]
        [Route("api/lots/create")]
        public IHttpActionResult AddLot(LotModel model)
        {
            var user = userManager.GetUserByName(User.Identity.Name);
            var lot = Mapper.Map<LotModel, LotDTO>(model);

            byte[] array = null;

            if (model.File != null)
                using (MemoryStream ms = new MemoryStream())
                {
                    model.File.InputStream.CopyTo(ms);
                    array = ms.GetBuffer();
                }

            lot.User = user;
            lot.Img = array;
            lotService.CreateLot(lot);

            return Ok("Lot created");
        }

        [HttpPost]
        [Authorize]
        [Route("api/lots/edit")]
        public IHttpActionResult EditLot(LotModel model)
        {
            lotService.EditLot(Mapper.Map<LotModel, LotDTO>(model));
            var lot = lotService.GetLot(model.Id);

            return Ok($"Lot {lot.Name} edited\nId = {lot.Id}");
        }

        [HttpDelete]
        [Authorize]
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

        [HttpGet]
        [Route("api/lots/{id}")]
        public LotModel GetLot(int id)
        {
            var lot = lotService.GetLot(id);

            return Mapper.Map<LotDTO, LotModel>(lot);
        }

        [HttpGet]
        [Route("api/categories/{id}/lots")]
        public IEnumerable<LotModel> GetLots(int id)
        {
            var lots = lotService.GetLotsForCategory(id);

            return Mapper.Map<IEnumerable<LotDTO>, IEnumerable<LotModel>>(lots);
        }

        [HttpPut]
        [Authorize(Roles="admin, moderator")]
        [Route("api/lots/{id}/verify")]
        public IHttpActionResult VerifyLot(int id)
        {
            lotService.VerifyLot(id);

            return Ok("Lot verified");
        }

        [HttpPost]
        [Authorize]
        [Route("api/lots/change")]
        public IHttpActionResult ChangeCategory(ChangeCategoryModel model)
        {
            lotService.ChangeLotCategory(model.LotId, model.CategoryId);

            return Ok("Category changed");
        }
    }
}