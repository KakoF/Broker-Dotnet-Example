using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace ServiceBus
{
    public class Connection
    {
        public IConnection Instance;

        private readonly string exchangeType = "topic";
        private readonly object syncRoot = new Object();

        public IModel _channel { get; protected set; }

        public Settings _settings { get; set; }

        public Connection(Settings settings)
        {
            _settings = settings;

            if (Instance == null)
            {
                lock (syncRoot)
                {
                    if (Instance == null)
                    {
                        Instance = new ConnectionFactory()
                        {
                            HostName = settings.HostName,
                            VirtualHost = settings.VirtualHost,
                            UserName = settings.UserName,
                            Password = settings.Password,
                            DispatchConsumersAsync = true
                        }.CreateConnection();
                    }
                }
            }
        }

        public IModel CreateChannel()
        {
            _channel = Instance.CreateModel();

            _channel.ExchangeDeclare(
                exchange: _settings.Exchange,
                type: exchangeType,
                durable: true,
                autoDelete: false);

            var args = new Dictionary<String, Object>
            {
                { "x-queue-type", "quorum" }
            };

            _channel.QueueDeclare(_settings.Fila, durable: true, autoDelete: false, exclusive: false, arguments: args);

            _channel.QueueBind(
                queue: _settings.Fila,
                exchange: _settings.Exchange,
                routingKey: _settings.RoutingKey);

            _channel.BasicQos(
                prefetchSize: 0,
                prefetchCount: _settings.MaximoConsumoMensagem,
                global: false);

            return _channel;

        }

        public void Dispose()
        {
            if (Instance != null)
            {
                _channel?.Dispose();
                Instance?.Dispose();
            }
        }
    }
}