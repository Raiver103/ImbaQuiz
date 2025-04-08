using ImbaQuiz.infrastructure.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace ImbaQuiz.API.Services
{
    public class LogSender : ILogSender
    {
        public void SendLog(string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "logs", durable: true, exclusive: false, autoDelete: false);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: "logs",
                basicProperties: null,
                body: body
            );
        }
    }
}
