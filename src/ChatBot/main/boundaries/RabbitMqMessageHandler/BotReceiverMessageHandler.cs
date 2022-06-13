using System.Globalization;
using ChatBot.Core.Boundaries.BotMessageHandlers;
using ChatBot.Core.Models;
using CsvHelper;
using RabbitMqMessageHandler.Settings;

namespace RabbitMqMessageHandler
{
    public class BotReceiverMessageHandler : IBotReceiverMessageHandler
    {
        private readonly IBotSendMessageHandler _sendMessageHandler;
        private readonly StockServiceConfiguration _serviceConfiguration;
        private readonly HttpClient _client = new HttpClient();
        public BotReceiverMessageHandler(IBotSendMessageHandler sendMessageHandler,
            StockServiceConfiguration serviceConfiguration)
        {
            _sendMessageHandler = sendMessageHandler;
            _serviceConfiguration = serviceConfiguration;
        }

        public async Task HandleMessage(MessageRequest message)
        {
            var command = message.Message.Split("=");


            switch (command[0])
            {
                case BotCommands.StockCommand:
                    var param = command[1];
                    try
                    {
                        await StockCommand(message, command);
                    }
                    catch (Exception ex)
                    {
                        _sendMessageHandler.SendMessage(CreateBotMessage(message, "Ups, looks like my service is not running. Try again later :c"));
                    }

                    break;

                default:
                    Console.WriteLine("Handle unknow commands here");
                    _sendMessageHandler.SendMessage(CreateBotMessage(message, "Sorry I could not understand the command. :c"));
                    break;
            }


        }

        private async Task StockCommand(MessageRequest message, string[] command)
        {

            var stockUrl = _serviceConfiguration.ServiceUrl.Replace("{{stock}}", command[1]);

            using (var response = await _client.GetAsync(stockUrl))
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var stock = csv.GetRecords<StockCsv>()
                    .ToList().FirstOrDefault();

                var botMessage = $"sorry we could not find the quote for the stock '{command[1]}'";

                if (stock.Close != "N/D")
                    botMessage = $"{command[1].ToUpper()} quote is ${stock.Close} per share";
                _sendMessageHandler.SendMessage(CreateBotMessage(message, botMessage));
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

        public static class BotCommands
        {
            public const string StockCommand = "/stock";
        }
    }
}
