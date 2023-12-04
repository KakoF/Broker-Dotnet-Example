using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using Service.Services;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Service.Consumers.Consumers
{
    public class ConsumidorStatusAlterado : ConsumidorBase
    {
        private readonly IEntidadeService _service;
        private readonly IPedidoStatusService _pedidoStatusService;
        private readonly ComputedPedidos _computed;
        public ConsumidorStatusAlterado(ILogger<ConsumidorStatusAlterado> logger,
            IConfiguration configuration, IEntidadeService service, IPedidoStatusService pedidoStatusService, ComputedPedidos computed)
            : base(logger, configuration, Consumidores.StatusAlterado)
        {
            _service = service;
            _pedidoStatusService = pedidoStatusService;
            _computed = computed;
        }

        public override async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var data = GetMensagem(e);
            if (data != null)
                await ContabilizarPedidoFinalizado(data);
        }

        private StatusAlteradoMessageModel GetMensagem(BasicDeliverEventArgs data)
        {
            string mensagem = Encoding.UTF8.GetString(data.Body.ToArray());
            try
            {
                StatusAlteradoMessageModel statusAlterado = JsonConvert.DeserializeObject<StatusAlteradoMessageModel>(mensagem);
                return statusAlterado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Consumidores.StatusAlterado}: não foi possivel DeserializeJObject | Mensagem: {mensagem}. RoutingKey: {data.RoutingKey}");
            }
            return null;
        }

        private async Task ContabilizarPedidoFinalizado(StatusAlteradoMessageModel data)
        {
            if (!await _service.EntidadeConfiguradaParaContador(data.EntidadeId))
                return;

            if (data.StatusNovoNaoFinalizado)
            {
                if (data.Incrementar())
                    _computed.IncrementarPedidosFinalizados(data.EntidadeId);
            }
            else
            {
                var finalizacao = await _pedidoStatusService.StatusFinalizacao(data.AtributosStatusNovo);

                if (data.Decrementar(finalizacao.Item2, finalizacao.Item1))
                    _computed.DecrementarPedidosFinalizados(data.EntidadeId);
            }
        }
    }
}