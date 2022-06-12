using ChatBot.Core.Boundaries.Persistence;
using ChatBot.Core.Models;

namespace ChatBot.Persistence.Repositories
{
    public class ChatRoomRepository: GenericRepository<Room>, IChatRoomRepository
    {
        public ChatRoomRepository(ChatDbContext chatDbContext) : base(chatDbContext)
        {
        }
    }
}
