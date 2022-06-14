using ChatBot.Commands;
using ChatBot.Core.Boundaries;
using ChatBot.Core.Models;

namespace BotCommandTest
{
    [TestFixture]
    public class BotStockCommandFacts
    {
        private StockServiceConfiguration _configuration;

        [SetUp]
        public void SetUp()
        {
            _configuration = new StockServiceConfiguration
            {
                ServiceUrl = "https://stooq.com/q/l/?s={{stock}}&f=sd2t2ohlcv&h&e=csv"
            };
        }

        [TestCase("/stock=AAPL.US")]
        [TestCase("/stock=ARSAUD")]
        [TestCase("/stock=AUDIDR")]
        [TestCase("/stock=BGNCZK")]
        [TestCase("/stock=CHFRON")]
        public  void With_Valid_Command_Throws_Nothing(string command)
        {
            ClientMessage clientMessage = new ClientMessage()
            {
                Message = command,
                RoomCode = Guid.NewGuid().ToString(),
                ClientUserName = "#Bot",
                SentOnUtc = DateTimeOffset.UtcNow
            };

            IBotCommand botCommand = new StockCommand(_configuration);

            Assert.That( async () => await botCommand.ExecuteCommand(clientMessage), Throws.Nothing);
        }

        [TestCase("/stock=")]
        [TestCase("")]
        public void With_Valid_Command_Returns_StockQuote(string command)
        {
            ClientMessage clientMessage = new ClientMessage()
            {
                Message = command,
                RoomCode = Guid.NewGuid().ToString(),
                ClientUserName = "#Bot",
                SentOnUtc = DateTimeOffset.UtcNow
            };

            IBotCommand botCommand = new StockCommand(_configuration);

            Assert.ThrowsAsync<FormatException>(() => botCommand.ExecuteCommand(clientMessage));
        }


        [TestCase("/stock=AAPL")]
        [TestCase("/stock=HELLO WORD")]
        public async Task With_Non_Existing_Command_Returns_Opps_Message(string command)
        {
            ClientMessage clientMessage = new ClientMessage()
            {
                Message = command,
                RoomCode = Guid.NewGuid().ToString(),
                ClientUserName = "#Bot",
                SentOnUtc = DateTimeOffset.UtcNow
            };

            IBotCommand botCommand = new StockCommand(_configuration);
           var result  = await botCommand.ExecuteCommand(clientMessage);

           Assert.That(result.Message,Contains.Substring("sorry we could not find the quote for the stock"));

        }
    }
}