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
            Database.Lots.Create(Mapper.Map<LotDTO, Lot>(entity));
            Database.Save();
        }

        public void RemoveLot(LotDTO entity)
        {
            Database.Lots.Delete((Mapper.Map<LotDTO, Lot>(entity)).Id);
            Database.Save();
        }

        public void RemoveLotFromCategory(LotDTO lot, CategoryDTO category)
        {
            Lot workLot = Mapper.Map<LotDTO, Lot>(lot);
            Category workCategory = Mapper.Map<CategoryDTO, Category>(category);
            if (workLot == null || workCategory == null)
                throw new ArgumentNullException();

            workLot.Categories.Remove(workCategory);
            workCategory.Lots.Remove(workLot);

            Database.Lots.Update(workLot);
            Database.Categories.Update(workCategory);
            Database.Save();
        }

        public void AddLotToCategory(LotDTO lot, CategoryDTO category)
        {
            Lot workLot = Mapper.Map<LotDTO, Lot>(lot);
            Category workCategory = Mapper.Map<CategoryDTO, Category>(category);
            if (workLot == null || workCategory == null)
                throw new ArgumentNullException();

            workLot.Categories.Add(workCategory);
            workCategory.Lots.Add(workLot);

            Database.Lots.Update(workLot);
            Database.Categories.Update(workCategory);
            Database.Save();
        }
    }
}
