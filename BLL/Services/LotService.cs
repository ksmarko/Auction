using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using System;
using DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Database.Lots.Update(Mapper.Map<LotDTO, Lot>(entity));
            Database.Save();
        }

        public void CreateLot(LotDTO entity)
        {
            Lot newLot = Mapper.Map<LotDTO, Lot>(entity);
            newLot.Categories.Add(Database.Categorys.Get(1));
            Database.Lots.Create(newLot);
            Database.Save();
        }

        public void RemoveLot(int lotId)
        {
            Lot workLot = Database.Lots.Find(x => x.Id == lotId).FirstOrDefault();
            if (workLot == null)
                throw new ArgumentNullException();
            
            Database.Lots.Delete(workLot.Id);
            Database.Save();
        }

        public void RemoveLotFromCategory(int lotId, int categoryId)
        {
            Lot workLot = Database.Lots.Find(x => x.Id == lotId).FirstOrDefault();
            Category workCategory = Database.Categorys.Find(x => x.Id == categoryId).FirstOrDefault();
            if (workLot == null || workCategory == null)
                throw new ArgumentNullException();

            workLot.Categories.Remove(workCategory);
            workCategory.Lots.Remove(workLot);

            if (workLot.Categories.Count == 0)
                AddLotToCategory(lotId, 1);

            Database.Lots.Update(workLot);
            Database.Categorys.Update(workCategory);
            Database.Save();
        }

        public void AddLotToCategory(int lotId, int categoryId)
        {
            Lot workLot = Database.Lots.Find(x => x.Id == lotId).FirstOrDefault();
            Category workCategory = Database.Categorys.Find(x => x.Id == categoryId).FirstOrDefault();
            if (workLot == null || workCategory == null)
                throw new ArgumentNullException();

            workLot.Categories.Add(workCategory);
            workCategory.Lots.Add(workLot);
                
            Database.Lots.Update(workLot);
            Database.Categorys.Update(workCategory);
            Database.Save();
        }

        public IEnumerable<LotDTO> GetAll()
        {
            return Mapper.Map<IEnumerable<Lot>, List<LotDTO>>(Database.Lots.GetAll());
        }

        public LotDTO GetLot(int LotId)
        {
            return Mapper.Map<Lot, LotDTO>(Database.Lots.Get(LotId));
        }
    }
}
