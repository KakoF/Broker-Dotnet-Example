using Dapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infra.Config;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Infra.Repositories
{
    [ExcludeFromCodeCoverage]
    public class PedidoStatusRepository : IPedidoStatusRepository
    {
        private readonly string _primaryDataBaseConnetion;

        public PedidoStatusRepository(IOptions<DatabaseConfig> config)
        {
            _primaryDataBaseConnetion = config.Value.PrimaryDataBase;
        }
        public async Task<IEnumerable<PedidosStatusEntity>> ObterStatusDeFinalizacao()
        {
            using var dbConnection = new SqlConnection(_primaryDataBaseConnetion);
            var data = await dbConnection.QueryAsync<PedidosStatusEntity>(@"select PEDIDO_STATUS_ID AS PedidoStatusId, Atributos from PedidosStatus where (Atributos & (1 | 2048)) <> 0");
            return data;

        }  
        
    }
}