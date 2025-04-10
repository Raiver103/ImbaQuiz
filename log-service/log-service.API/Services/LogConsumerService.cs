using log_service.API.Configurations;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace log_service.API.Services
{
    public class LogConsumerService : BackgroundService
    {
        private readonly RabbitMqSettings _rabbitSettings;
        private IConnection _connection;
        private IModel _channel;

        public LogConsumerService(IOptions<RabbitMqSettings> rabbitMqOptions)
        {
            _rabbitSettings = rabbitMqOptions.Value;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitSettings.HostName,
                UserName = _rabbitSettings.UserName,
                Password = _rabbitSettings.Password 
            }; 

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _rabbitSettings.QueueName, durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Log.Information("📥 Log received: {Message}", message);
            };
  
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
