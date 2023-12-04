using Domain.Entities;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IEntidadeMetodoRepository
    {
        Task<IEnumerable<EntidadeConfiguradaModel>> ObterEntidadesConfiguradasTransbordoAsync();
    }
}
