using AutoMapper;
using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Identity.Entities;
using DAL.Identity.Interfaces;
using DAL.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserManager : IUserManager
    {
        IUnitOfWorkIdentity DatabaseIdentity { get; set; }
        IUnitOfWork DatabaseDomain { get; set; }

        public UserManager(IUnitOfWorkIdentity uowi, IUnitOfWork uow)
        {
            DatabaseIdentity = uowi;
            DatabaseDomain = uow;
        }
        
        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            var user = await DatabaseIdentity.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                var result = await DatabaseIdentity.UserManager.CreateAsync(user, userDto.Password);

                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                await DatabaseIdentity.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                User clientProfile = new User { Id = user.Id, Name = userDto.UserName };
                DatabaseIdentity.ClientManager.Create(clientProfile);
                await DatabaseIdentity.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var appUsers = DatabaseIdentity.UserManager.Users;
            var list = new List<UserDTO>();

            if (appUsers != null)
                foreach (var appUser in appUsers)
                    list.Add(CreateUserDTO(appUser));

            return list;
        }

        public void Dispose()
        {
            DatabaseIdentity.Dispose();
        }

        public async Task<Tuple<ClaimsIdentity, ClaimsIdentity>> FindAsync(string username, string password)
        {
            var appUser = await DatabaseIdentity.UserManager.FindAsync(username, password);

            //if (appUser == null)
            //throw new AuthException("invalid_grant", "The user name or password is incorrect.");

            ClaimsIdentity oAuthIdentity = await DatabaseIdentity.UserManager.CreateIdentityAsync(appUser, OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await DatabaseIdentity.UserManager.CreateIdentityAsync(appUser, CookieAuthenticationDefaults.AuthenticationType);

            return new Tuple<ClaimsIdentity, ClaimsIdentity>(oAuthIdentity, cookiesIdentity);
        }

        private UserDTO CreateUserDTO(ApplicationUser user)
        {
            return new UserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Name = user.User.Name,
                Role = GetRoleForUser(user.Id),
                Lots = Mapper.Map<IEnumerable<Lot>, ICollection<LotDTO>>(DatabaseDomain.Lots.Find(x => x.User.Id == user.Id))
            };
        }

        private string GetRoleForUser(string id)
        {
            var user = DatabaseIdentity.UserManager.FindById(id);
            var roleId = user.Roles.Where(x => x.UserId == user.Id).Single().RoleId;
            var role = DatabaseIdentity.RoleManager.Roles.Where(x => x.Id == roleId).Single().Name;

            return role;
        }
    }
}
