using System.Text;
using ChatBot.Core.Boundaries.BotMessageHandlers;
using ChatBot.Core.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqMessageHandler.Settings;

namespace RabbitMqMessageHandler
{
    public sealed class BotWorker: BackgroundService
    {
        private readonly RabbitMqConfiguration _configuration;
        private readonly IBotReceiverMessageHandler _botReceiverMessageHandler;
        private IConnection _connection;
        private IModel _channel;

        public BotWorker(RabbitMqConfiguration configuration, 
         
            IBotReceiverMessageHandler botReceiverMessageHandler)
        {
            _configuration = configuration;
            _botReceiverMessageHandler = botReceiverMessageHandler;
            InitializeRabbitMqListener();

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var clientMessage = JsonConvert.DeserializeObject<MessageRequest>(content);

                _botReceiverMessageHandler.HandleMessage(clientMessage);

                _channel.BasicAck(ea.DeliveryTag, false);

            };
            _channel.BasicConsume(_configuration.BotQueueName, false, consumer);

            return Task.CompletedTask;
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration.HostName,
                UserName = _configuration.UserName,
                Password = _configuration.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _configuration.BotQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }
    }
}
