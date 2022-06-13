using ChatBot.Core.Models;

namespace ChatBot.Core.Boundaries
{
    public interface IBotCommand
    {
        string Command { get; set; }
        Task<MessageRequest> ExecuteCommand(MessageRequest message);
    }
}
