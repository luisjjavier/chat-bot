using ChatBot.Core.Models;

namespace ChatBot.Core.Services.Contracts
{
    public interface ITokenHandler
    {
        string GenerateToken(User user);

    }
}
