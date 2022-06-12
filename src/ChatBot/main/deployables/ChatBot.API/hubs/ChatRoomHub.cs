using Microsoft.AspNetCore.SignalR;

namespace ChatBot.API.hubs
{
    public class ChatRoomHub: Hub
    {
        public async Task SendMessage(string chatRoomCode, string user, string message)
        {
            await Clients.Group(chatRoomCode).SendAsync(HubConstants.ON_MSG_RECVD, "");
        }

        public async Task EnrollUserToChatRoom(string chatRoomCode)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomCode);
            await Clients.Group(chatRoomCode).SendAsync(HubConstants.ON_USR_ENRLLMENT_RECVD, $"{Context.ConnectionId} has joined the group {chatRoomCode}.");
        }
    }

}
