using Core.Interfaces.Resiliencia;
using Domain.Entity;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infra.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ContadorPedidosTransbordadosService : IContadorPedidosTransbordadosService
    {
        private readonly IContadorPedidosTransbordadosRepository _repository;
        private readonly IEntidadeService _entidadeService;
        private readonly IAsyncPolicies _asyncPolicies;
        private readonly NLog.ILogger _elasticContador = LogManager.GetLogger("ElasticContador");
        private readonly ILogger<ContadorPedidosTransbordadosService> _logger;
        private readonly ComputedPedidos _computed;

        private readonly int _quantidade;
        private readonly int _intervalo;
        public ContadorPedidosTransbordadosService(IContadorPedidosTransbordadosRepository repository,
            IEntidadeService entidadeService,
            IAsyncPolicies asyncPolicies,
            ILogger<ContadorPedidosTransbordadosService> logger,
            IOptions<RetryConfig> config,
            ComputedPedidos computed)
        {
            _repository = repository;
            _entidadeService = entidadeService;
            _asyncPolicies = asyncPolicies;
            _logger = logger;
            _quantidade = config.Value.Quantidade;
            _intervalo = config.Value.Intervalo;
            _computed = computed;
        }
        public async Task Salvar()
        {
            var entidadeConfigurada = await _entidadeService.GetEntidadesConfiguradasAsync();
            foreach (var configuracao in entidadeConfigurada)
            {
                var pedidosFinalizados = _computed.BuscarPedidosFinalizados(configuracao.EntidadeId);
                var pedidosTransbordados = _computed.BuscarPedidosTransbordados(configuracao.EntidadeId);

                if (pedidosFinalizados == 0 && pedidosTransbordados == 0)
                    continue;

                var entity = new ContadorPedidosTransbordadosEntity(configuracao.EntidadeId, pedidosFinalizados, pedidosTransbordados);
                try
                {
                    await _asyncPolicies.GerarRetryPolicy(_quantidade, nameof(Executar), TimeSpan.FromMilliseconds(_intervalo))
                    .ExecuteAsync(async () => await Executar(entity));

                    _computed.RemoverPedidosFinalizados(configuracao.EntidadeId, pedidosFinalizados);
                    _computed.RemoverPedidosTransbordados(configuracao.EntidadeId, pedidosTransbordados);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao executar operação para contador transbordo referente aos dados. {@Information}", new { entity });
                }
            }
        }

        private async Task Executar(ContadorPedidosTransbordadosEntity data)
        {
            if (data != null)
            {
                _logger.LogDebug($"Encontrou pedidos para serem contados. Pedidos não finalizados:{data.PedidosNaoFinalizados}; Pedidos transbordados:{data.PedidosTransbordados}");
                await _repository.UpdateAsync(DateTime.Now.Date, data.EntidadeId, data);
                _elasticContador.Info("Métricas Contador Transbordo. {@Information}", data);
            }
        }
    }
}
