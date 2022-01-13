﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServeNObserve.DataModels.Request;
using ServeNObserve.Domain.Models;

namespace ServeNObserve.Domain.Repositories
{
    public interface IUserRepository
    {
        User getThisUser(); // get user from context 
        User GetUserById(int id); // attachToUser for JWT
        Task<List<User>> GetUsersAsync();
        Task<List<User>> GetUserByName(string name);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByMailThrowAsync(string email);
        Task<User> GetUserByMailAsync(string email);
        Task<User> LoginUserAsync(LoginRequest request);
        Task UpdateUser(User user);
        Task<bool> Create(User user);
        Task<bool> Save();

    }
}
