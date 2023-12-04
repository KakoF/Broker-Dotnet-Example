using Domain.Entity;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IContadorPedidosTransbordadosRepository
    {
        Task UpdateAsync(DateTime data, int entidadeId, ContadorPedidosTransbordadosEntity entity);
    }
}
