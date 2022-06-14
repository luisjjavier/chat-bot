using ChatBot.Core.Models;

namespace ChatBot.Core.Services.Contracts
{
    public interface IChatRoomService
    {
        Task CreateNewRoomAsync(Room room);
        Task<string> ProcessMessage(ClientMessage clientMessage);
        Task<ICollection<ClientMessage>> GetChatRoomMessages(Guid roomCode);
    }
}
