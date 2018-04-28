using AutoMapper;
using System;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Entities;
using System.Collections.Generic;

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
            Database.Categories.Update(Mapper.Map<CategoryDTO, Category>(entity));
            Database.Save();
        }

        public void RemoveCategory(CategoryDTO entity)
        {
            Category category = Mapper.Map<CategoryDTO, Category>(entity);
            if(category.Lots!= null)
                foreach (var el in category.Lots)
                {
                    el.Categories.Remove(category);
                    Database.Lots.Update(el);
                }

            Database.Categories.Delete(category.Id);
            Database.Save();
        }

        public void CreateCategory(CategoryDTO entity)
        {
            Database.Categories.Create(Mapper.Map<CategoryDTO, Category>(entity));
            Database.Save();
        }
    }
}
