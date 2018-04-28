using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ILotService
    {
        //functional for lots (create, edit, remove etc) and categories (add category, add lot to category and so)
        void EditLot(LotDTO entity);
        void CreateLot(LotDTO entity);
        void RemoveLot(LotDTO entity);
        //function RemoveLotCategory and AddLotCategor use to work with category that lot contein
        void RemoveLotCategory(LotDTO lot, CategoryDTO category);
        void AddLotCategory(LotDTO lot, CategoryDTO category);
        void Dispose();
    }
}
