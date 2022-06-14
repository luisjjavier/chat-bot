using ChatBot.Core.Boundaries.BotMessageHandlers;
using ChatBot.Core.Boundaries.Persistence;
using ChatBot.Core.Models;
using ChatBot.Core.Services.Contracts;
using ChatBot.Core.Validations;
using FluentValidation;

namespace ChatBot.Core.Services
{
    public sealed class ChatRoomService : IChatRoomService
    {
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IRepository<Message> _messageRepository;
        private readonly IBotSendMessageHandler _sendMessageHandler;

        public ChatRoomService(IChatRoomRepository chatRoomRepository,
            IRepository<Message> messageRepository, IBotSendMessageHandler sendMessageHandler)
        {
            _chatRoomRepository = chatRoomRepository;
            _messageRepository = messageRepository;
            _sendMessageHandler = sendMessageHandler;

        }
        public async Task CreateNewRoomAsync(Room room)
        {
            RoomValidator roomValidator = new RoomValidator();
            await roomValidator.ValidateAndThrowAsync(room);
            await _chatRoomRepository.InsertAsync(room);
        }

        public async Task<string> ProcessMessage(ClientMessage clientMessage)
        {
            if (IsBotCommand(clientMessage.Message))
            {
                _sendMessageHandler.SendMessage(clientMessage);

                return "FinnBot is processing your message";
            }

            var room = await _chatRoomRepository.FirstAsNoTracking(room =>
                room.Code == new Guid(clientMessage.RoomCode));
            var message = new Message
            {
                FromUser = clientMessage.ClientUserName,
                SentTime = clientMessage.SentOnUtc,
                RawMessage = clientMessage.Message,
                RoomId = room.Id
            };

            await _messageRepository.InsertAsync(message);

            return message.RawMessage;
        }

        public async Task<ICollection<ClientMessage>> GetChatRoomMessages(Guid roomCode)
        {
            var room = await _chatRoomRepository.FirstAsNoTracking(room => room.Code == roomCode);

            var messages = _messageRepository.WhereAsNoTracking(message => message.RoomId == room.Id)
                 .OrderBy(x => x.SentTime).Take(50)
                 .Select(x => new ClientMessage
                 {
                     Message = x.RawMessage,
                     RoomCode = room.ToString()!,
                     ClientUserName = x.FromUser,
                     SentOnUtc = x.SentTime
                 }).ToList();

            return messages;
        }
        private bool IsBotCommand(string message)
        {
            return message.Contains("/");
        }
    }

}
