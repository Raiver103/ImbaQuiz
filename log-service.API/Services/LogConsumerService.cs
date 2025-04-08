using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace log_service.API.Services
{
    public class LogConsumerService : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "logs", durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Log.Information("📥 Log received: {Message}", message);
            };

            _channel.BasicConsume(queue: "logs", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        //protected override Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        //    _connection = factory.CreateConnection();
        //    _channel = _connection.CreateModel();

        //    _channel.QueueDeclare(queue: "logs", durable: true, exclusive: false, autoDelete: false);

        //    var consumer = new EventingBasicConsumer(_channel);
        //    consumer.Received += (model, ea) =>
        //    {
        //        var body = ea.Body.ToArray();
        //        var message = Encoding.UTF8.GetString(body);

        //        Log.Information("[LOG] {Message}", message);
        //    };

        //    _channel.BasicConsume(queue: "logs", autoAck: true, consumer: consumer);

        //    return Task.CompletedTask;
        //}

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
