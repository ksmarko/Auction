using AutoMapper;
using Auction.BLL.DTO;
using Auction.BLL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Auction.WEB.Models;

namespace Auction.WEB.Controllers
{
    public class ManagementController : ApiController
    {
        readonly IUserManager userManager;
        readonly ICategoryService categoryService;
        readonly ITradeService tradeService;
        readonly ILotService lotService;

        public ManagementController(IUserManager userManager, ICategoryService categoryService, ITradeService tradeService, ILotService lotService)
        {
            this.userManager = userManager;
            this.categoryService = categoryService;
            this.tradeService = tradeService;
            this.lotService = lotService;
        }

        [HttpGet]
        [Route("api/users")]
        [Authorize(Roles = "admin")]
        public IEnumerable<UserModel> Users()
        {
            var users = Mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserModel>>(userManager.GetUsers());

            return users;
        }

        [HttpGet]
        [Route("api/roles")]
        [Authorize(Roles = "admin")]
        public IEnumerable<string> GetRoles()
        {
            return userManager.GetRoles();
        }

        [HttpPost]
        [Route("api/users/edit")]
        [Authorize(Roles = "admin")]
        public async Task<IHttpActionResult> EditRoles(EditRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            await userManager.EditRole(model.UserId, model.Role);
            return Ok("Role edited");
        }

        [HttpPost]
        [Route("api/categories/create")]
        [Authorize(Roles = "admin, moderator")]
        public IHttpActionResult AddCategory(CategoryModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            var category = Mapper.Map<CategoryModel, CategoryDTO>(model);
            categoryService.CreateCategory(category);

            return Ok("Category created");
        }

        [HttpPost]
        [Route("api/categories/edit")]
        [Authorize(Roles = "admin, moderator")]
        public IHttpActionResult EditCategory(CategoryModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            categoryService.EditCategory(Mapper.Map<CategoryModel, CategoryDTO>(model));
            return Ok("Category edited");
        }

        [HttpDelete]
        [Route("api/categories/{id}")]
        [Authorize(Roles = "admin, moderator")]
        public IHttpActionResult DeleteCategory(int id)
        {
            var category = categoryService.GetCategory(id);
            categoryService.RemoveCategory(id);

            return Ok($"Category {category.Name} removed\nId = {category.Id}");
        }

        [HttpPut]
        [Authorize(Roles = "admin, moderator")]
        [Route("api/lots/{id}/verify")]
        public IHttpActionResult VerifyLot(int id)
        {
            lotService.VerifyLot(id);
            tradeService.StartTrade(id);

            return Ok("Lot verified. Trade started");
        }
    }
}