using ChatBot.Core.Models;

namespace ChatBot.Core.Services.Contracts
{
    public interface IUserService
    {
        Task RegisterAUser(User user, string password);

        Task<User> LoginAsync(LoginRequest user);
    }
}
