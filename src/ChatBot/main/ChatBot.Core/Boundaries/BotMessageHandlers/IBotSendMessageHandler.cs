using ChatBot.Core.Models;

namespace ChatBot.Core.Boundaries.BotMessageHandlers
{
    public interface IBotSendMessageHandler
    {
        void SendMessage(ClientMessage clientMessage);
    }
}
