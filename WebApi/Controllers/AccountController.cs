using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class AccountController : ApiController
    {
        readonly IUserManager userManager;

        private IAuthenticationManager AuthenticationManager
        {
            get => Request.GetOwinContext().Authentication;
        }

        public AccountController(IUserManager userManager) => this.userManager = userManager;

        [HttpPost]
        [Route("api/account/login")]
        //[ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await userManager.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return Ok();
                }
            }
            return BadRequest();
        }

        public IHttpActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return Ok();
        }

        [HttpPost]
        [Route("api/account/register")]
        //[ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Role = "user"
                };

                OperationDetails operationDetails = await userManager.Create(userDto);
                if (operationDetails.Succedeed)
                {
                    ClaimsIdentity claim = await userManager.Authenticate(userDto);
                    if (claim == null)
                    {
                        ModelState.AddModelError("", "Неверный логин или пароль.");
                    }
                    else
                    {
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                        return Ok(model);
                    }
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return BadRequest();
        }
    }
}
