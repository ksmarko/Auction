﻿using AutoMapper;
using Auction.BLL.DTO;
using Auction.BLL.Exceptions;
using Auction.BLL.Infrastructure;
using Auction.BLL.Interfaces;
using Auction.DAL.Entities;
using Auction.DAL.Identity.Entities;
using Auction.DAL.Identity.Interfaces;
using Auction.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auction.BLL.Services
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

                await DatabaseIdentity.UserManager.AddToRoleAsync(user.Id, "user");
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

            if (appUser == null)
                throw new UserNotFoundException("The user name or password is incorrect.");

            ClaimsIdentity oAuthIdentity = await DatabaseIdentity.UserManager.CreateIdentityAsync(appUser, OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await DatabaseIdentity.UserManager.CreateIdentityAsync(appUser, CookieAuthenticationDefaults.AuthenticationType);

            return new Tuple<ClaimsIdentity, ClaimsIdentity>(oAuthIdentity, cookiesIdentity);
        }

        public UserDTO GetUserByName(string name)
        {
            ApplicationUser appUser = DatabaseIdentity.UserManager.FindByName(name);

            if (appUser == null)
                throw new UserNotFoundException();

            return CreateUserDTO(appUser);
        }

        public async Task EditRole(string userId, string newRoleName)
        {
            var user = await DatabaseIdentity.UserManager.FindByIdAsync(userId);

            if (user == null)
                throw new UserNotFoundException();

            var oldRole = GetRoleForUser(userId);

            if (oldRole != newRoleName)
            {
                await DatabaseIdentity.UserManager.RemoveFromRoleAsync(userId, oldRole);
                await DatabaseIdentity.UserManager.AddToRoleAsync(userId, newRoleName);

                await DatabaseIdentity.UserManager.UpdateAsync(user);
            }
        }

        public IEnumerable<string> GetRoles()
        {
            return DatabaseIdentity.RoleManager.Roles.Select(x => x.Name);
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
