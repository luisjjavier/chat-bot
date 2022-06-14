using ChatBot.Core.Models;

namespace ChatBot.Core.Boundaries.BotMessageHandlers
{
    public interface IBotReceiverMessageHandler
    {
        Task HandleMessage(ClientMessage clientMessage);
    }
}
