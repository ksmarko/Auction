using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using System;
using DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using BLL.Infrastructure;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        IUnitOfWork Database { get; set; }

        public CategoryService(IUnitOfWork uow)
        {
            Database = uow;
        }
        
        public void Dispose()
        {
            Database.Dispose();
        }

        public void EditCategory(CategoryDTO entity)
        {
            Database.Categorys.Update(Mapper.Map<CategoryDTO, Category>(entity));
            Database.Save();
        }

        public void RemoveCategory(int categoryId)
        {
            if (categoryId == 1)
                throw new AuctionException("Yuo can`t delete default category");
            Category category = Database.Categorys.Find(x => x.Id == categoryId).FirstOrDefault();
            if (category == null)
                throw new ArgumentNullException();
            /*if(category.Lots!= null)
                foreach (var el in category.Lots)
                {
                    el.Categories.Remove(category);
                    Database.Lots.Update(el);
                }*/
                
            Database.Categorys.Delete(category.Id);
            Database.Save();
        }

        public void CreateCategory(CategoryDTO entity)
        {
            Database.Categorys.Create(Mapper.Map<CategoryDTO, Category>(entity));
            Database.Save();
        }

        public IEnumerable<CategoryDTO> GetAllCategory()
        {
            return Mapper.Map<IEnumerable<Category>, List<CategoryDTO>>(Database.Categorys.GetAll());
        }

        public CategoryDTO GetCategory(int categoryId)
        {
            return Mapper.Map<Category, CategoryDTO>(Database.Categorys.Get(categoryId));
        }
    }
}
