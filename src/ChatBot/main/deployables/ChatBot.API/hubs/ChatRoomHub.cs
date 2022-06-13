using ChatBot.Core.Models;
using ChatBot.Core.Services.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace ChatBot.API.hubs
{
    public class ChatRoomHub: Hub
    {
        private readonly IChatRoomService _chatRoomService;
        public ChatRoomHub(IChatRoomService chatRoomService)
        {
            _chatRoomService = chatRoomService;
        }
         
        public async Task SendMessage(MessageRequest messageRequest)
        {
          string processedMessage =   await _chatRoomService.ProcessMessage(messageRequest);
            await Clients.Group(messageRequest.RoomCode).SendAsync(HubConstants.ON_MSG_RECVD, messageRequest);
        }

        public async Task EnrollUserToChatRoom(string chatRoomCode, string username)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomCode);
            var messageRequest = new MessageRequest
            {
                Message = $"{username} has joined the group.",
                RoomCode = chatRoomCode,
                ClientUserName = "#system",
                SentOnUtc = DateTimeOffset.UtcNow
            };

            await Clients.Group(chatRoomCode).SendAsync(HubConstants.ON_USR_ENRLLMENT_RECVD, messageRequest);
        }
    }

}
