using Domain.Entities;
using Domain.Interfaces.Cache;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class PedidoStatusService : IPedidoStatusService
    {
        private readonly IPedidoStatusRepository _repository;
        private readonly ICache _cache;

        public PedidoStatusService(IPedidoStatusRepository repository, ICache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<(bool, bool)> StatusFinalizacao(int? atributo)
        {
            if (atributo == null)
                return (false, false);

            var listaPedidoStatus = await _cache.GetOrCreateAsync(
                "PedidosStatusFinalizados",
                async () => await _repository.ObterStatusDeFinalizacao()
            );

            return FinalizacaoAutomatica(listaPedidoStatus.FirstOrDefault(p => p.Atributos == atributo));

        }

        private (bool, bool) FinalizacaoAutomatica(PedidosStatusEntity pedidoStatus)
        {
            if (pedidoStatus == null)
            {
                return (false, false);
            }
            bool finalizacao = Convert.ToBoolean(pedidoStatus.Atributos & 1);
            bool automatica = Convert.ToBoolean(pedidoStatus.Atributos & 2048);

            return (finalizacao, automatica);
        }
    }
}
