using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class HomeController : ApiController
    {
        readonly ICategoryService categoryService;

        public HomeController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        // GET api/home
        public IEnumerable<CategoryModel> Get()
        {
            return Mapper.Map<IEnumerable<CategoryDTO>, IEnumerable<CategoryModel>>(categoryService.GetAllCategories());
        }

        // GET api/home/5
        public string Get(int id)
        {
            return categoryService.GetCategory(id).Name;
        }
    }
}
