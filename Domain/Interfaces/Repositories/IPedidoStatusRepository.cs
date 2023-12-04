using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IPedidoStatusRepository
    {
        Task<IEnumerable<PedidosStatusEntity>> ObterStatusDeFinalizacao();
    }
}
