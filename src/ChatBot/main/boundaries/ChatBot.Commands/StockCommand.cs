using System.Globalization;
using ChatBot.Core.Boundaries;
using ChatBot.Core.Models;
using CsvHelper;

namespace ChatBot.Commands
{
    public class StockCommand: IBotCommand
    {
        private readonly StockServiceConfiguration _serviceConfiguration;
        private readonly HttpClient _client = new HttpClient();
        public StockCommand(StockServiceConfiguration serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration;

        }
        public string Command { get; set; } = "/stock";
        public async Task<ClientMessage> ExecuteCommand(ClientMessage clientMessage)
        {
            try
            {
                return await ExecuteStockCommand(clientMessage);
            }
            catch (Exception e)
            {
                throw new FormatException(e.Message);
            }

        }

        private async Task<ClientMessage> ExecuteStockCommand(ClientMessage clientMessage)
        {
            var command = clientMessage.Message.Split("=");
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
                return CreateBotMessage(clientMessage, botMessage);
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