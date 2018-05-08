using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class ValuesController : ApiController
    {
        readonly IUserManager userManager;
        readonly ICategoryService categoryService;

        public ValuesController(IUserManager userManager, ICategoryService categoryService)
        {
            this.userManager = userManager;
            this.categoryService = categoryService;
        }

        // GET api/values
        [HttpGet]
        [Route("api/account/id")]
        public string GetUser(int id)
        {
            return userManager.GetUsers().ElementAt(id).UserName;
        }

        // GET api/values/5
        [HttpGet]
        [Route("api/index")]
        public string[] Get()
        {
            return categoryService.GetAllCategories().Select(x => x.Name).ToArray();
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
