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

        public async Task HandleMessage(MessageRequest message)
        {
            var command = message.Message.Split("=");

            if (_botCommands.ContainsKey(command[0]))
            {
                _sendMessageHandler.SendMessage(await _botCommands[command[0]].ExecuteCommand(message));
            }
            else
            {
                _sendMessageHandler.SendMessage(CreateBotMessage(message, "Sorry I could not understand the command. :c"));
            }
        }
        private MessageRequest CreateBotMessage(MessageRequest message, string botMessage)
        {
            return new MessageRequest
            {
                Message = botMessage,
                RoomCode = message.RoomCode,
                ClientUserName = "#Bot",
                SentOnUtc = DateTimeOffset.UtcNow
            };
        }

    }
}
