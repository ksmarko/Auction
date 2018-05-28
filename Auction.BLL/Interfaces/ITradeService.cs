using Auction.BLL.DTO;
using System.Collections.Generic;

namespace Auction.BLL.Interfaces
{
    /// <summary>
    /// Service for work with trades
    /// </summary>
    public interface ITradeService
    {
        /// <summary>
        /// Starts trade
        /// </summary>
        /// <param name="lotId">Lot Id</param>
        /// <exception cref="ArgumentNullException">When lot not found</exception>
        /// <exception cref="AuctionException">When trade with this lot has alredy began or lot is not verified</exception>
        void StartTrade(int lotId);

        /// <summary>
        /// Rates for lot
        /// </summary>
        /// <param name="tradeId">Trade Id</param>
        /// <param name="userId">New User Id</param>
        /// <param name="price">New Price</param>
        /// <exception cref="ArgumentNullException">When trade not found</exception>
        /// <exception cref="AuctionException">When owner try to rate his lot, when trade is over or when new price is smaller then previous</exception>
        void Rate(int tradeId, string userId, double price);

        /// <summary>
        /// Gets all trades
        /// </summary>
        /// <returns>Returns list of trades</returns>
        IEnumerable<TradeDTO> GetAllTrades();

        /// <summary>
        /// Gets trade by Id
        /// </summary>
        /// <param name="id">Trade Id</param>
        /// <returns></returns>
        TradeDTO GetTrade(int id);

        /// <summary>
        /// Gets trade by lot
        /// </summary>
        /// <param name="id">Lot Id</param>
        TradeDTO GetTradeByLot(int id);

        /// <summary>
        /// Gets all trades that user has lose
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Returns list of trades</returns>   
        IEnumerable<TradeDTO> GetUserLoseTrades(string userId);

        /// <summary>
        /// Gets all trades that user has won
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Returns list of trades</returns>
        IEnumerable<TradeDTO> GetUserWinTrades(string userId);

        void Dispose();
    }
}
