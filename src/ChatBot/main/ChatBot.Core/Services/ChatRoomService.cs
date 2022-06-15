using ChatBot.Core.Boundaries.BotMessageHandlers;
using ChatBot.Core.Boundaries.Persistence;
using ChatBot.Core.Models;
using ChatBot.Core.Services.Contracts;
using ChatBot.Core.Validations;
using FluentValidation;

namespace ChatBot.Core.Services
{
    /// <summary>
    /// A service which manage all chat room behavior
    /// </summary>
    public sealed class ChatRoomService : IChatRoomService
    {
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IRepository<Message> _messageRepository;
        private readonly IBotSendMessageHandler _sendMessageHandler;

        /// <summary>
        /// Creates a new instance of <see cref="ChatRoomService"/>
        /// </summary>
        /// <param name="chatRoomRepository"></param>
        /// <param name="messageRepository"></param>
        /// <param name="sendMessageHandler"></param>
        public ChatRoomService(IChatRoomRepository chatRoomRepository,
            IRepository<Message> messageRepository, IBotSendMessageHandler sendMessageHandler)
        {
            _chatRoomRepository = chatRoomRepository;
            _messageRepository = messageRepository;
            _sendMessageHandler = sendMessageHandler;

        }

        /// <summary>
        /// Create a new chat room
        /// </summary>
        /// <param name="room"></param>
        public async Task CreateNewRoomAsync(Room room)
        {
            RoomValidator roomValidator = new RoomValidator();
            await roomValidator.ValidateAndThrowAsync(room);
            await _chatRoomRepository.InsertAsync(room);
        }

        /// <summary>
        /// Process a client message and send it to a bot or db
        /// </summary>
        /// <param name="clientMessage"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get the last 50 messages from chat room given a room code
        /// </summary>
        /// <param name="roomCode"></param>
        /// <returns></returns>
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
