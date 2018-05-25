﻿using BLL.DTO;
using BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserManager : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        IEnumerable<UserDTO> GetUsers();
        Task<Tuple<ClaimsIdentity, ClaimsIdentity>> FindAsync(string username, string password);
    }
}
