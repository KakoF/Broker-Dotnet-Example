using Domain.Interfaces.Cache;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class EntidadeService : IEntidadeService
    {
        private readonly IEntidadeMetodoRepository _entidadeMetodoRepository;
        private readonly ICache _cache;

        public EntidadeService(IEntidadeMetodoRepository entidadeMetodoRepository, ICache cache)
        {
            _entidadeMetodoRepository = entidadeMetodoRepository;
            _cache = cache;
        }
        public async Task<IEnumerable<EntidadeConfiguradaModel>> GetEntidadesConfiguradasAsync()
        {
            return await _cache.GetOrCreateAsync(
                "EntidadesConfiguradasTransbordo",
                async () => await _entidadeMetodoRepository.ObterEntidadesConfiguradasTransbordoAsync()
            );
        }

        public async Task<bool> EntidadeConfiguradaParaContador(int entidadeId)
        {
            var configurados = await GetEntidadesConfiguradasAsync();
            return configurados.Any(p => p.EntidadeId == entidadeId);
        }
    }
}
