using System;
using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using src.models;

namespace src
{
    public class RabbitEndpoint : IRabbitEndpoint
    {
        private IConnection connection;
        private IModel channel;
        private readonly IRouter _router;


        public RabbitEndpoint(IRouter router)
        {
            _router = router;
        }
        public void StartListening()
        {
            var rabbitHost = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME") ?? "rabbit";
            var rabbitUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
            var rabbitPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest";
            var environmentMode = Environment.GetEnvironmentVariable("ENVIRONMENT_MODE") ?? "dev";

            // Factory
            var factory = new ConnectionFactory { HostName = rabbitHost, UserName = rabbitUser, Password = rabbitPassword };
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            
            // Connection
            connection = TryOpenConnection(factory);

            // Channel
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "rabbit_event_stream", type: "fanout", durable: true);

            // Queue
            var queueName = channel.QueueDeclare($"something_happened_{environmentMode}", durable: true, exclusive: false, autoDelete: false, arguments: null).QueueName;
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            channel.QueueBind(queue: queueName, exchange: "rabbit_event_stream", routingKey: "");

            
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                // Here we send all the messages to the router.
                _router.RouteAsync(message, () =>
                 {
                     channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                 });
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        private IConnection TryOpenConnection(ConnectionFactory factory, int attempt = 0)
        {

            try
            {
                return factory.CreateConnection();
            }
            catch (Exception e)
            {
                attempt++;
                if (attempt < 10)
                {
                    System.Threading.Thread.Sleep(5000);
                    return TryOpenConnection(factory, attempt);
                }

                throw;
            }
        }

        public void StopListening()
        {
            if (channel != null && connection != null)
            {
                channel.Close(200, "Goodbye");
                connection.Close();
            }
        }

        public bool IsConnectionsOpen()
        {
            return connection.IsOpen && channel.IsOpen;
        }
    }
}