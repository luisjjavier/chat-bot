using ChatBot.Core.Boundaries.Persistence;
using ChatBot.Core.Models;
using ChatBot.Core.Services.Contracts;

namespace ChatBot.Core.Services
{
    public sealed class ChatRoomService: IChatRoomService
    {
        private readonly IChatRoomRepository _chatRoomRepository;

        public ChatRoomService(IChatRoomRepository chatRoomRepository )
        {
            _chatRoomRepository = chatRoomRepository;

        }
        public async Task CreateNewRoomAsync(Room room)
        {
            await _chatRoomRepository.InsertAsync(room);
        }
    }
}
