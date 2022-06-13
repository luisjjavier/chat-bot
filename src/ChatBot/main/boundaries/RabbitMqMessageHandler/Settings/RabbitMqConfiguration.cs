﻿namespace RabbitMqMessageHandler.Settings
{
    public sealed class RabbitMqConfiguration
    {
        public string HostName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public string BotQueueName { get; set; }
        public string ChatQueueName { get; set; }
    }
}
