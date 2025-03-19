using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TingtelAssessment.Application.Interfaces;

namespace TingtelAssessment.Infrastructure.Services
{
    public class RabbitMQSettings
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class RabbitMQService : IMessageQueueService
    {
        private readonly RabbitMQSettings _settings;

        public RabbitMQService(IOptions<RabbitMQSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task PublishAsync<T>(string queueName, T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            using (var connection = await factory.CreateConnectionAsync())
            using (var channel = await connection.CreateChannelAsync())
            {
                //channel.QueueDeclare(
                //    queue: queueName,
                //    durable: true,
                //    exclusive: false,
                //    autoDelete: false,
                //    arguments: null);

                //var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                //var properties = channel.CreateBasicProperties();
                //properties.Persistent = true;

                //channel.BasicPublish(
                //    exchange: "",
                //    routingKey: queueName,
                //    basicProperties: properties,
                //    body: body);
            }
        }
    }
}
