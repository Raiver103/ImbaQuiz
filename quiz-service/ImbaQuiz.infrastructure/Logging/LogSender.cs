using ImbaQuiz.Domain.Interfaces;
using ImbaQuiz.infrastructure.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace ImbaQuiz.API.Services
{
    public class LogSender : ILogSender
    {
        private readonly RabbitMqSettings _rabbitMqSettings;
        public LogSender(IOptions<RabbitMqSettings> rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings.Value;
        }
        public void SendLog(string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqSettings.HostName,
                UserName = _rabbitMqSettings.UserName,
                Password = _rabbitMqSettings.Password
                
            }; 

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _rabbitMqSettings.QueueName, durable: true, exclusive: false, autoDelete: false);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                routingKey: _rabbitMqSettings.QueueName,
                                basicProperties: null,
                                body: body);
        }
    }

}
