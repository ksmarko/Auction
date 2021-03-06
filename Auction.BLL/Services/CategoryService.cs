﻿using AutoMapper;
using Auction.BLL.DTO;
using Auction.BLL.Interfaces;
using Auction.DAL.Interfaces;
using System;
using Auction.DAL.Entities;
using System.Collections.Generic;
using Auction.BLL.Exceptions;

namespace Auction.BLL.Services
{
    /// <summary>
    /// Service for work with categories
    /// </summary>
    public class CategoryService : ICategoryService
    {
        /// <summary>
        /// Represents domain database
        /// </summary>
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Creates service
        /// </summary>
        /// <param name="uow">UnitOfWork</param>
        public CategoryService(IUnitOfWork uow)
        {
            Database = uow;
        }
        
        public void Dispose()
        {
            Database.Dispose();
        }

        /// <summary>
        /// Changes category name
        /// </summary>
        /// <param name="entity">Category with new data</param>
        /// <exception cref="ArgumentNullException">When category not found</exception>
        public void EditCategory(CategoryDTO entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            var temp = Database.Categories.Get(entity.Id);

            if (temp == null)
                throw new ArgumentNullException();

            temp.Name = entity.Name;

            Database.Categories.Update(temp);
            Database.Save();
        }

        /// <summary>
        /// Removes category
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <exception cref="AuctionException">When try to delete default category</exception>
        public void RemoveCategory(int id)
        {
            if (id == 1)
                throw new AuctionException("You can't delete default category");

            var category = Database.Categories.Get(id);

            if (category == null)
                throw new ArgumentNullException();

            if (category.Lots != null)
                foreach (var el in category.Lots)
                    el.Category = Database.Categories.Get(1);

            Database.Categories.Delete(category.Id);
            Database.Save();
        }

        /// <summary>
        /// Creates new category
        /// </summary>
        /// <param name="entity">Category</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void CreateCategory(CategoryDTO entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            Database.Categories.Create(new Category { Name = entity.Name});
            Database.Save();
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>Returns list of categories</returns>
        public IEnumerable<CategoryDTO> GetAllCategories()
        {
            return Mapper.Map<IEnumerable<Category>, List<CategoryDTO>>(Database.Categories.GetAll());
        }

        /// <summary>
        /// Get category by Id
        /// </summary>
        /// <param name="id">Category Id</param>
        public CategoryDTO GetCategory(int id)
        {
            return Mapper.Map<Category, CategoryDTO>(Database.Categories.Get(id));
        }
    }
}
