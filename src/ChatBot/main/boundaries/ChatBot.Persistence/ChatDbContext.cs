using ChatBot.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Persistence
{
    public  sealed class ChatDbContext: IdentityDbContext<User>
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options): base(options)
        {
        }
    }
}
