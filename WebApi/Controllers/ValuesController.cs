using BLL.DTO;
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
        readonly ICategoryService categoryService;

        public ValuesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        // GET api/values
        public IEnumerable<CategoryDTO> Get()
        {
            return categoryService.GetAllCategories();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return categoryService.GetCategory(id).Name;
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
        public IHttpActionResult Delete(int id)
        {
            categoryService.RemoveCategory(id);
            return Ok("Successfully deleted");
        }
    }
}
