using ChatBot.Core.Models;
using ChatBot.Core.Services.Contracts;
using ChatBot.Core.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Triplex.Validations;

namespace ChatBot.Core.Services
{
    /// <summary>
    /// Manage all user requirements
    /// </summary>
    public sealed class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task RegisterAUser(User user, string password)
        {
            Arguments.NotNull(user, nameof(user));
            Arguments.NotNull(password, nameof(password));

            var userValidator = new UserValidator();
            await userValidator.ValidateAndThrowAsync(user);
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(",", result.Errors.Select(e => e.Description)));
            }
        }

        /// <summary>
        /// Starting from a login request, it indicates if a user can login
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        public async Task<User> LoginAsync(LoginRequest loginRequest)
        {
            Arguments.NotNull(loginRequest, nameof(loginRequest));

            var user = Arguments.NotNull(await _userManager.FindByNameAsync(loginRequest.UserName), nameof(loginRequest), "Invalid user or password");
            bool isValidPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            Arguments.CompliesWith(isValidPassword, "Invalid user or password", "Invalid user or password");

            return user;
        }
    }
}
