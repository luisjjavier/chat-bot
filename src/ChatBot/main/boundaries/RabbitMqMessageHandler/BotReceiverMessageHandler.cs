using ChatBot.Commands;
using ChatBot.Core.Boundaries;
using ChatBot.Core.Boundaries.BotMessageHandlers;
using ChatBot.Core.Models;

namespace RabbitMqMessageHandler
{
    public class BotReceiverMessageHandler : IBotReceiverMessageHandler
    {
        private readonly IBotSendMessageHandler _sendMessageHandler;
        private readonly IDictionary<string, IBotCommand> _botCommands = new Dictionary<string, IBotCommand>();

        public BotReceiverMessageHandler(IBotSendMessageHandler sendMessageHandler, StockServiceConfiguration serviceConfiguration)
        {
            _sendMessageHandler = sendMessageHandler;
            var command = new StockCommand(serviceConfiguration);
            _botCommands.Add(command.Command, command);
            
        }

        public async Task HandleMessage(ClientMessage clientMessage)
        {
            var command = clientMessage.Message.Split("=");

            if (_botCommands.ContainsKey(command[0]))
            {
                _sendMessageHandler.SendMessage(await _botCommands[command[0]].ExecuteCommand(clientMessage));
            }
            else
            {
                _sendMessageHandler.SendMessage(CreateBotMessage(clientMessage, "Sorry I could not understand the command. :c"));
            }
        }
        private ClientMessage CreateBotMessage(ClientMessage clientMessage, string botMessage)
        {
            return new ClientMessage
            {
                Message = botMessage,
                RoomCode = clientMessage.RoomCode,
                ClientUserName = "#Bot",
                SentOnUtc = DateTimeOffset.UtcNow
            };
        }

    }
}
