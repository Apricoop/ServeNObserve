using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServeNObserve.DataModels.Request;
using ServeNObserve.DataModels.Response;
using ServeNObserve.Domain.Models;

namespace ServeNObserve.Domain.Services
{
    public interface IUserService
    {
        Task<List<UserResponse>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        User GetUserAuth();
        User GetUserById(int id);
        Task<bool> CreateUser(CreateUserRequest request);
        //Task<bool> ConfirmEmail();
        Task<UserLoginResponse> LoginUser(LoginRequest request);
        //Task<bool> ResetUserPassword(ForgotPasswordRequest request);
        //Task<bool> ChangeUserPassword(ChangePasswordRequest changePasswordRequest);
        Task<bool> UpdateUser(UpdateUserRequest request);
        Task<bool> DeleteUserById(int id);
    }
}
