using System;
using System.Collections.Generic;
using System.Linq;
using Auction.BLL.DTO;

namespace Auction.BLL.Interfaces
{
    /// <summary>
    /// Service for work with lots
    /// </summary>
    public interface ILotService
    {
        /// <summary>
        /// Changes lot data
        /// </summary>
        /// <param name="entity">Lot with new data</param>
        /// <exception cref="ArgumentNullException">When lot not found</exception>
        /// <exception cref="AuctionException">When trade started</exception>
        void EditLot(LotDTO entity);

        /// <summary>
        /// Creates lot
        /// </summary>
        /// <param name="entity">New lot</param>
        /// <exception cref="ArgumentNullException">When input entity is null or lot haven't owner(user)</exception>
        void CreateLot(LotDTO entity);

        /// <summary>
        /// Removes lot
        /// </summary>
        /// <param name="id">Lot Id</param>
        /// <exception cref="ArgumentNullException">When lot not found</exception>
        void RemoveLot(int id);

        /// <summary>
        /// Gets lots list
        /// </summary>
        /// <returns>Returns list of lots</returns>
        IEnumerable<LotDTO> GetAllLots();

        /// <summary>
        /// Gets lot by id
        /// </summary>
        /// <param name="id">Lot Id</param>
        LotDTO GetLot(int id);

        /// <summary>
        /// Changes lot category
        /// </summary>
        /// <param name="lotId">Lot Id</param>
        /// <param name="categoryId">Category Id</param>
        /// <exception cref="ArgumentNullException">When lot or/and category not found</exception>
        void ChangeLotCategory(int lotId, int categoryId);

        /// <summary>
        /// Varifies lot
        /// </summary>
        /// <param name="id">Lot Id</param>
        /// <exception cref="ArgumentNullException">When lot not found</exception>
        void VerifyLot(int id);

        void Dispose();

        /// <summary>
        /// Gets lots from category
        /// </summary>
        /// <param name="categoryId">Category Id</param>
        /// <returns>Returns list of lots in category</returns>
        IEnumerable<LotDTO> GetLotsForCategory(int categoryId);
    }
}
