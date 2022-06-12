using ChatBot.Core.Models;
using ChatBot.Core.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Triplex.Validations;

namespace ChatBot.Core.Services
{
    public sealed class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task RegisterAUser(User user, string password)
        {
            Arguments.NotNull(user, nameof(user));
            Arguments.NotNull(password, nameof(password));

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(",", result.Errors.Select(e => e.Description)));
            }
        }

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
