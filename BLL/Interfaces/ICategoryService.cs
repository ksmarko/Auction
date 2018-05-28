using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    /// <summary>
    /// Service for work with categories
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Creates new category
        /// </summary>
        /// <param name="entity">Category</param>
        /// <exception cref="ArgumentNullException"></exception>
        void CreateCategory(CategoryDTO entity);

        /// <summary>
        /// Removes category
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <exception cref="AuctionException">When try to delete default category</exception>
        void RemoveCategory(int id);

        /// <summary>
        /// Changes category name
        /// </summary>
        /// <param name="entity">Category with new data</param>
        /// <exception cref="ArgumentNullException">When category not found</exception>
        void EditCategory(CategoryDTO entity);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>Returns list of categories</returns>
        IEnumerable<CategoryDTO> GetAllCategories();

        /// <summary>
        /// Get category by Id
        /// </summary>
        /// <param name="id">Category Id</param>
        CategoryDTO GetCategory(int id);

        void Dispose();
    }
}
