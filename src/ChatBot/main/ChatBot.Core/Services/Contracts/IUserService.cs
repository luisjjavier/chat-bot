using ChatBot.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatBot.Core.Services.Contracts
{
    public interface IUserService
    {
        Task RegisterAUser(User user, string password);
    }
}
