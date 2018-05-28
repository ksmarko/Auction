using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Auction.WEB.Models;

namespace Auction.WEB.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        readonly IUserManager userManager;

        IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public AccountController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var user = new UserDTO() { UserName = model.Email, Email = model.Email, Password = model.Password};
            var result = await userManager.Create(user);

            if (!result.Succedeed)
                return BadRequest(result.Message);

            return Ok();
        }
    }
}
