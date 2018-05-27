using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using System;
using DAL.Entities;
using System.Collections.Generic;
using BLL.Infrastructure;

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
            var temp = Database.Categories.Get(entity.Id);

            if (temp == null)
                throw new ArgumentNullException();

            temp.Name = entity.Name;

            Database.Categories.Update(temp);
            Database.Save();
        }

        public void RemoveCategory(int id)
        {
            if (id == 1)
                throw new AuctionException("You can't delete default category");

            var category = Database.Categories.Get(id);

            if (category == null)
                throw new ArgumentNullException();

            Database.Categories.Delete(category.Id);
            Database.Save();
        }

        public void CreateCategory(CategoryDTO entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            Database.Categories.Create(new Category { Name = entity.Name});
            Database.Save();
        }

        public IEnumerable<CategoryDTO> GetAllCategories()
        {
            return Mapper.Map<IEnumerable<Category>, List<CategoryDTO>>(Database.Categories.GetAll());
        }

        public CategoryDTO GetCategory(int id)
        {
            return Mapper.Map<Category, CategoryDTO>(Database.Categories.Get(id));
        }
    }
}
