using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ICategoryService
    {
        void CreateCategory(CategoryDTO entity);
        void RemoveCategory(int id);
        void EditCategory(CategoryDTO entity);
        IEnumerable<CategoryDTO> GetAllCategories();
        CategoryDTO GetCategory(int id);
        void Dispose();
    }
}
