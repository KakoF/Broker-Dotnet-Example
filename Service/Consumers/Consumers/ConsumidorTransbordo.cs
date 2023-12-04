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
    public class ConsumidorTransbordo : ConsumidorBase
    {
        private readonly IEntidadeService _service;
        private readonly ComputedPedidos _computed;
        public ConsumidorTransbordo(ILogger<ConsumidorTransbordo> logger,
            IConfiguration configuration, IEntidadeService service, ComputedPedidos computed)
            : base(logger, configuration, Consumidores.Transbordo)
        {
            _service = service;
            _computed = computed;
        }

        public override async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var data = GetMensagem(e);
            if (data != null)
                await ContabilizarPedidoTransbordado(data);

        }

        private TransbordoMessageModel GetMensagem(BasicDeliverEventArgs data)
        {
            string mensagem = Encoding.UTF8.GetString(data.Body.ToArray());
            try
            {
                TransbordoMessageModel transbordo = JsonConvert.DeserializeObject<TransbordoMessageModel>(mensagem);
                return transbordo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Consumidores.Transbordo}: não foi possivel DeserializeJObject | Mensagem: {mensagem}. RoutingKey: {data.RoutingKey}");
            }
            return new TransbordoMessageModel();
        }

        private async Task ContabilizarPedidoTransbordado(TransbordoMessageModel data)
        {
            if (await _service.EntidadeConfiguradaParaContador(data.EntidadeID))
                _computed.IncrementarPedidosTransbordados(data.EntidadeID);
        }
    }
}