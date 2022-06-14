using System.Text;
using ChatBot.Core.Boundaries.BotMessageHandlers;
using ChatBot.Core.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMqMessageHandler.Settings;

namespace RabbitMqMessageHandler
{
    public sealed class BotSendMessageHandler: IBotSendMessageHandler
    {
        private readonly RabbitMqConfiguration _configuration;
        private IConnection _connection;

        public BotSendMessageHandler(RabbitMqConfiguration configuration)
        {
            _configuration = configuration;

        }
        public void SendMessage(ClientMessage clientMessage)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                channel.QueueDeclare(queue: _configuration.ChatQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var json = JsonConvert.SerializeObject(clientMessage);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: _configuration.ChatQueueName, basicProperties: null, body: body);
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configuration.HostName,
                    UserName = _configuration.UserName,
                    Password = _configuration.Password,
                    Port = 5672,
                    AutomaticRecoveryEnabled = true,
                    TopologyRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
                    ContinuationTimeout = TimeSpan.FromMinutes(5),
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
                return true;

            CreateConnection();

            return _connection != null;
        }

    }
}
