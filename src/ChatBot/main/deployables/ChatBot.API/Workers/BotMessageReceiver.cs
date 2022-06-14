using System.Text;
using ChatBot.API.hubs;
using ChatBot.Core.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqMessageHandler.Settings;

namespace ChatBot.API.Workers
{
    public class BotMessageReceiver: BackgroundService
    {
        private readonly RabbitMqConfiguration _rabbitMqConfiguration;
        private readonly IHubContext<ChatRoomHub> _hubContext;
        private IConnection _connection;
        private IModel _channel;

        public BotMessageReceiver(RabbitMqConfiguration rabbitMqConfiguration, IHubContext<ChatRoomHub> hubContext)
        {
            _rabbitMqConfiguration = rabbitMqConfiguration;
            _hubContext = hubContext;
            InitializeRabbitMqListener();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var clientMessage = JsonConvert.DeserializeObject<MessageRequest>(content);

                await _hubContext.Clients.Group(clientMessage.RoomCode)
                    .SendAsync(HubConstants.ON_MSG_RECVD,  clientMessage);

                _channel.BasicAck(ea.DeliveryTag, false);

            };
            _channel.BasicConsume(_rabbitMqConfiguration.ChatQueueName, false, consumer);

            return Task.CompletedTask;
        }
        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqConfiguration.HostName,
                UserName = _rabbitMqConfiguration.UserName,
                Password = _rabbitMqConfiguration.Password,
                Port = 5672
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _rabbitMqConfiguration.ChatQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }
    }
}
