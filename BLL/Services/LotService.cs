using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using System;
using DAL.Entities;
using System.Collections.Generic;
using BLL.Infrastructure;

namespace BLL.Services
{
    public class LotService : ILotService
    {
        IUnitOfWork Database { get; set; }

        public LotService(IUnitOfWork uow)
        {
            Database = uow;
        }
                     
        public void Dispose()
        {
            Database.Dispose();
        }

        public void EditLot(LotDTO entity)
        {
            if(entity == null)
                throw new ArgumentNullException();

            var lot = Database.Lots.Get(entity.Id);

            if (lot == null)
                throw new ArgumentNullException();

            if (lot.IsVerified)
                throw new AuctionException("You can`t change the information about the lot after the start of the bidding");

            lot.Name = entity.Name;
            lot.Description = entity.Description;
            lot.Img = entity.Img;
            lot.TradeDuration = entity.TradeDuration;
            

            Database.Lots.Update(lot);
            Database.Save();
        }

        public void CreateLot(LotDTO entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            var newLot = new Lot()
            {
                Name = entity.Name,
                Price = entity.Price,
                Description = entity.Description,
                Img = entity.Img,
                TradeDuration = entity.TradeDuration,
                User = entity.User == null ? null : Database.Users.Get(entity.User.Id)
            };
            
            newLot.Category = Database.Categories.Get(1);

            Database.Lots.Create(newLot);
            Database.Save();
        }

        public void RemoveLot(int id)
        {
            Lot lot = Database.Lots.Get(id);

            if (lot == null)
                throw new ArgumentNullException();
            
            Database.Lots.Delete(lot.Id);
            Database.Save();
        }

        public void ChangeLotCategory(int lotId, int categoryId)
        {
            Lot lot = Database.Lots.Get(lotId);
            Category category = Database.Categories.Get(categoryId);

            if (lot == null || category == null)
                throw new ArgumentNullException();

            lot.Category = category;
                
            Database.Lots.Update(lot);
            Database.Save();
        }

        public IEnumerable<LotDTO> GetAllLots()
        {
            return Mapper.Map<IEnumerable<Lot>, List<LotDTO>>(Database.Lots.GetAll());
        }

        public LotDTO GetLot(int id)
        {
            return Mapper.Map<Lot, LotDTO>(Database.Lots.Get(id));
        }

        public void VerifyLot(int id)
        {
            Lot lot = Database.Lots.Get(id);
                
            if (lot == null)
                throw new ArgumentNullException();

            lot.IsVerified = true;

            Database.Lots.Update(lot);
            Database.Save();
        }
    }
}
