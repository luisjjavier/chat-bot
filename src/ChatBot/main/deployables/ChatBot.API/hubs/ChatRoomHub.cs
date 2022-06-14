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
         
        public async Task SendMessage(ClientMessage clientMessage)
        {
            string processedMessage =   await _chatRoomService.ProcessMessage(clientMessage);
            await Clients.Group(clientMessage.RoomCode).SendAsync(HubConstants.ON_MSG_RECVD, clientMessage);
        }

        public async Task EnrollUserToChatRoom(string chatRoomCode, string username)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomCode);
            var messageRequest = new ClientMessage
            {
                Message = $"{username} has joined the group.",
                RoomCode = chatRoomCode,
                ClientUserName = "#system",
                SentOnUtc = DateTimeOffset.UtcNow
            };
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomCode);
            await Clients.Group(chatRoomCode).SendAsync(HubConstants.ON_USR_ENRLLMENT_RECVD, messageRequest);
        }
    }

}
