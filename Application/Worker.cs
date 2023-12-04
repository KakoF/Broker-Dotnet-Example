using Domain.Interfaces.Consumers;
using Domain.Interfaces.Services;
using Infra.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ComputedPedidos _computed;
        private readonly IConsumidoresRabbit _consumidores;
        private readonly IContadorPedidosTransbordadosService _service;
        private readonly IOptions<FlushConfig> _flushConfig;
        private long _tempo = 0;

        public Worker(ILogger<Worker> logger, IConsumidoresRabbit consumidores, IContadorPedidosTransbordadosService service, ComputedPedidos computed, IOptions<FlushConfig> config)
        {
            _logger = logger;
            _consumidores = consumidores;
            _service = service;
            _computed = computed;
            _flushConfig = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Worker Contador Transbordo rodando: {time}", DateTimeOffset.Now);
            await _consumidores.IniciarAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10000, stoppingToken);
                _tempo += 10000;
                if (_computed.PedidosComputadosFinalizados >= _flushConfig.Value.QuantidadeMaxima || _tempo >= _flushConfig.Value.Intervalo)
                {
                    _logger.LogDebug($"Execução do flush: {_computed.PedidosComputadosFinalizados} - {_tempo}");
                    await _service.Salvar();
                    _tempo = 0;
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker Contador Transbordo parando, executando flush: {time}", DateTimeOffset.Now);
            await _service.Salvar();
            _logger.LogInformation("Worker Contador Transbordo parado: {time}", DateTimeOffset.Now);
        }
    }
}
