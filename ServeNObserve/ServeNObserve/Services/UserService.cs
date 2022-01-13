using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServeNObserve.DataModels.Request;
using ServeNObserve.DataModels.Response;
using ServeNObserve.Domain.Models;
using ServeNObserve.Domain.Repositories;
using ServeNObserve.Domain.Services;
using ServeNObserve.Shared;

namespace ServeNObserve.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _config;

        public UserService(IOptions<AppSettings> appSettings, IConfiguration config, IUserRepository userRepository, ITokenGenerator tokenGenerator, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
            _config = config;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
        }

        // Kullanicinin kendi sifresini degistirmesi icin
        public async Task<bool> ChangeUserPassword(ChangePasswordRequest changePasswordRequest)
        {
            var user = _userRepository.getThisUser();
            if (changePasswordRequest.NewPassword != changePasswordRequest.ConfirmNewPassword)
                throw new ApiException(new Error {Message = "Parolalar eşleşmiyor lütfen kontrol ediniz"});

            var passTuple = PassHasherGenerator.HashPassword(changePasswordRequest.NewPassword, null);
            user.HashedPassword = passTuple.Item2;
            user.Salt = passTuple.Item1;
            await _userRepository.UpdateUser(user);
            return true;
        }

        // lazim olursa diye koyuyorum
        //public async Task<bool> ConfirmEmail()
        //{
        //    var user = _userRepository.getThisUser();
        //    //user.EmailConfirmed = true;
        //    return await _userRepository.Save();
        //}

        public async Task<bool> CreateUser(CreateUserRequest request)
        {
            var isExist = await _userRepository.GetUserByMailAsync(request.Email);
            if (isExist != null)
            {
                _logger.LogInformation("Girilen mail ile kayıt zaten mevcut");
                throw new ApiException(new Error
                {
                    Message = "Bu e-mail hesabı ile kayıtlı bir kullanıcı sistemde mevcut"
                });
            }

            var passTuple = PassHasherGenerator.HashPassword(request.Password, null);
            var newUser = new User(request, passTuple);
            await _userRepository.Create(newUser);
            return true;
        }

        public async Task<bool> DeleteUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return false;

            // isterseniz userRepository classinda db den remove ederek silinebilir.
            user.isActive = false;
            return await _userRepository.Save();
        }

        public User GetUserAuth()
        {
            return _userRepository.getThisUser();
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<List<UserResponse>> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            return users.Select(user => new UserResponse {User = user}).ToList();
        }

        public async Task<UserLoginResponse> LoginUser(LoginRequest request)
        {
            var user = await _userRepository.LoginUserAsync(request);
            var token = _tokenGenerator.GenerateToken(user);
            return new UserLoginResponse(user, token);
        }

        //public async Task<bool> ResetUserPassword(ForgotPasswordRequest request)
        //{
        //    var user = await _userRepository.GetUserByMailThrowAsync(request.Email);

        //    var newPassword = PasswordGenerator.GenerateRandomPassword(8);
        //    var passTuple = PassHasherGenerator.HashPassword(newPassword, null);
        //    user.HashedPassword = passTuple.Item2;
        //    user.Salt = passTuple.Item1;

        //    MailSender.SendMailAfterNewPassword(request.Email, newPassword);
        //    await _userRepository.UpdateUser(user);
        //    return true;
        //}

        public async Task<bool> UpdateUser(UpdateUserRequest request)
        {
            var user = _userRepository.getThisUser();
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            
            await _userRepository.UpdateUser(user);
            return true;
        }
    }
}
