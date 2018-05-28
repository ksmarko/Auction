using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using System;
using DAL.Entities;
using System.Collections.Generic;
using BLL.Exceptions;

namespace BLL.Services
{
    /// <summary>
    /// Service for work with lots
    /// </summary>
    public class LotService : ILotService
    {
        /// <summary>
        /// Represents domain database
        /// </summary>
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Creates service
        /// </summary>
        /// <param name="uow">UnitOfWork</param>
        public LotService(IUnitOfWork uow)
        {
            Database = uow;
        }
                     
        public void Dispose()
        {
            Database.Dispose();
        }

        /// <summary>
        /// Change lot name or/and descripyion, imeg, trade duration
        /// </summary>
        /// <param name="entity">Category with new data</param>
        /// <exception cref="ArgumentNullException">When lot not found</exception>
        /// <exception cref="AuctionException">When trade start</exception>
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

        /// <summary>
        /// Create lot
        /// </summary>
        /// <param name="entity">New lot</param>
        /// <exception cref="ArgumentNullException">When input entity is null or lot have now owner(user)</exception>
        public void CreateLot(LotDTO entity)
        {
            if (entity == null || entity.User == null)
                throw new ArgumentNullException();
                
            var newLot = new Lot()
            {
                Name = entity.Name,
                Price = entity.Price,
                Description = entity.Description,
                Img = entity.Img,
                TradeDuration = entity.TradeDuration,
                User = Database.Users.Get(entity.User.Id),
                Category = entity.Category == null ? Database.Categories.Get(1) : Database.Categories.Get(entity.Category.Id)
            };

            Database.Lots.Create(newLot);
            Database.Save();
        }

        /// <summary>
        /// Remove lot
        /// </summary>
        /// <param name="id">Lot Id</param>
        /// <exception cref="ArgumentNullException">If lot not found</exception>
        public void RemoveLot(int id)
        {
            Lot lot = Database.Lots.Get(id);

            if (lot == null)
                throw new ArgumentNullException();
            
            Database.Lots.Delete(lot.Id);
            Database.Save();
        }

        /// <summary>
        /// Change lot category
        /// </summary>
        /// <param name="lotId">Lot Id</param>
        /// <param name="categoryId">Category Id</param>
        /// <exception cref="ArgumentNullException">When lot or/and category not found</exception>
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

        /// <summary>
        /// Get lots list
        /// </summary>
        /// <returns>Returns list of lots</returns>
        public IEnumerable<LotDTO> GetAllLots()
        {
            return Mapper.Map<IEnumerable<Lot>, IEnumerable<LotDTO>>(Database.Lots.GetAll());
        }
        
        /// <summary>
        /// Get lots from category
        /// </summary>
        /// <param name="categoryId">Category Id</param>
        /// <returns>Return list of lots in category</returns>
        public IEnumerable<LotDTO> GetLotsForCategory(int categoryId)
        {
            return Mapper.Map<IEnumerable<Lot>, IEnumerable<LotDTO>>(Database.Lots.Find(x => x.CategoryId == categoryId));
        }

        /// <summary>
        /// Get lot
        /// </summary>
        /// <param name="id">Lot Id</param>
        public LotDTO GetLot(int id)
        {
            return Mapper.Map<Lot, LotDTO>(Database.Lots.Get(id));
        }

        /// <summary>
        /// Varify lot
        /// </summary>
        /// <param name="id">Lot Id</param>
        /// <exception cref="ArgumentNullException">When lot not found</exception>
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
