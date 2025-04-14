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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitSettings.HostName,
                UserName = _rabbitSettings.UserName,
                Password = _rabbitSettings.Password
            };

            int retryCount = 0;
            while (!stoppingToken.IsCancellationRequested && retryCount < 10)
            {
                try
                {
                    _connection = factory.CreateConnection();
                    _channel = _connection.CreateModel();
                    break;
                }
                catch (Exception ex)
                {
                                        Log.Warning("Failed to connect to RabbitMQ: {Message}", ex.Message); 

                    retryCount++;
                    await Task.Delay(5000, stoppingToken);
                }
            }
 
            if (_channel == null)
            {                
                Log.Error("Failed to establish connection with RabbitMQ after 10 attempts");

                return;
            }
 
            _channel.QueueDeclare(queue: _rabbitSettings.QueueName, durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Log.Information("📥 Log received: {Message}", message);
            };

            _channel.BasicConsume(queue: _rabbitSettings.QueueName, autoAck: true, consumer: consumer);
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
