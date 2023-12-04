using Domain.Interfaces.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceBus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Consumers.Consumers
{
    public abstract class ConsumidorBase : IConsumidor
    {
        protected readonly ILogger<ConsumidorBase> _logger;
        protected readonly List<ConsumerChannel> _consumersChannel;
        protected readonly Connection _connection;
        protected readonly Settings _settings;
        protected readonly string _consumidor;

        protected ConsumidorBase(ILogger<ConsumidorBase> logger, IConfiguration configuration, string consumidor)
        {
            _consumersChannel = new List<ConsumerChannel>();
            _settings = new Settings(consumidor, configuration);
            _connection = new Connection(_settings);
            _logger = logger;
            _consumidor = consumidor;
        }

        public void Consumir()
        {
            _logger.LogDebug($"Iniciando {_consumidor}. Consumidores: {_settings.QtdeConsumidores}.");

            var channel = _connection.CreateChannel();
            for (int i = 0; i < _settings.QtdeConsumidores; i++)
            {
                _logger.LogDebug($"Iniciando o consumo das mensagens para o serviço {_consumidor}. Consumer {i}");

                AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;


                channel.BasicConsume(_settings.Fila, true, consumer);
            }

            _consumersChannel.Add(new ConsumerChannel(_consumidor, channel.ChannelNumber, channel));
        }

        public abstract Task Consumer_Received(object sender, BasicDeliverEventArgs e);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Cleanup
            if (disposing)
            {
                // Cleanup
                if (_consumersChannel != null)
                {
                    foreach (var item in _consumersChannel)
                    {
                        item.Model?.Dispose();
                    }
                }

                _connection?.Dispose();
            }
        }
    }
}