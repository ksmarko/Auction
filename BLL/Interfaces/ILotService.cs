using System;
using System.Collections.Generic;
using System.Linq;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ILotService
    {
        void EditLot(LotDTO entity);
        void CreateLot(LotDTO entity);
        void RemoveLot(int id);
        IEnumerable<LotDTO> GetAllLots();
        LotDTO GetLot(int id);
        void ChangeLotCategory(int lotId, int categoryId);
        void VerifyLot(int id);
        void Dispose();
    }
}
