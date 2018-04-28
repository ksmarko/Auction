using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ICategoryService
    {
        void CreateCategory(CategoryDTO entity);
        void RemoveCategory(CategoryDTO entity);
        void EditCategory(CategoryDTO entity);
        void Dispose();
    }
}
