using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize(Roles = "admin")]
    public class ManagementController : ApiController
    {
        readonly IUserManager userManager;
        readonly ICategoryService categoryService;

        public ManagementController(IUserManager userManager, ICategoryService categoryService)
        {
            this.userManager = userManager;
            this.categoryService = categoryService;
        }

        [HttpGet]
        [Route("api/users")]
        public IEnumerable<UserModel> Users()
        {
            var users = Mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserModel>>(userManager.GetUsers());

            return users;
        }

        [HttpGet]
        [Route("api/roles")]
        public IEnumerable<string> GetRoles()
        {
            return userManager.GetRoles();
        }

        [HttpPost]
        [Route("api/users/edit")]
        public async Task EditRoles(EditRoleModel model)
        {
            await userManager.EditRole(model.UserId, model.Role);
        }

        [HttpPost]
        [Route("api/categories/create")]
        public IHttpActionResult AddCategory(CategoryModel model)
        {
            var category = Mapper.Map<CategoryModel, CategoryDTO>(model);
            categoryService.CreateCategory(category);

            return Ok("Category created");
        }

        [HttpPost]
        [Route("api/categories/edit")]
        public IHttpActionResult EditCategory(CategoryModel model)
        {
            categoryService.EditCategory(Mapper.Map<CategoryModel, CategoryDTO>(model));

            return Ok("Category edited");
        }

        [HttpDelete]
        [Route("api/categories/{id}")]
        public IHttpActionResult DeleteCategory(int id)
        {
            var category = categoryService.GetCategory(id);
            categoryService.RemoveCategory(id);

            return Ok($"Category {category.Name} removed\nId = {category.Id}");
        }
    }
}