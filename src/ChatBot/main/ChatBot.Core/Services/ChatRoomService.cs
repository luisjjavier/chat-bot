using ChatBot.Core.Boundaries.Persistence;
using ChatBot.Core.Models;
using ChatBot.Core.Services.Contracts;

namespace ChatBot.Core.Services
{
    public sealed class ChatRoomService: IChatRoomService
    {
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IRepository<Message> _messageRepository;

        public ChatRoomService(IChatRoomRepository chatRoomRepository, IRepository<Message> messageRepository)
        {
            _chatRoomRepository = chatRoomRepository;
            _messageRepository = messageRepository;

        }
        public async Task CreateNewRoomAsync(Room room)
        {
            await _chatRoomRepository.InsertAsync(room);
        }

        public async Task<string> ProcessMessage(MessageRequest messageRequest)
        {
            var room = await _chatRoomRepository.FirstAsNoTracking(room =>
                room.Code == new Guid(messageRequest.RoomCode));
            Message message = new Message()
            {
                FromUser = messageRequest.ClientUserName,
                SentTime = messageRequest.SentOnUtc,
                RawMessage = messageRequest.Message,
                RoomId = room.Id
            };

           await _messageRepository.InsertAsync(message);

           return message.RawMessage;
        }

        public async Task<ICollection<MessageRequest>> GetChatRoomMessages(Guid roomCode)
        {
            var room = await _chatRoomRepository.FirstAsNoTracking(room => room.Code == roomCode);

           var messages =  _messageRepository.WhereAsNoTracking(message => message.RoomId == room.Id)
                .OrderBy(x => x.SentTime).Take(50)
                .Select(x => new MessageRequest
                {
                    Message = x.RawMessage,
                    RoomCode = room.ToString()!,
                    ClientUserName = x.FromUser,
                    SentOnUtc = x.SentTime
                }).ToList();

           return messages;
        }
    }
}
