using Dapper;
using Domain.Entity;
using Domain.Interfaces.Repositories;
using Infra.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Infra.Repositories
{
    [ExcludeFromCodeCoverage]
    public class ContadorPedidosTransbordadosRepository : IContadorPedidosTransbordadosRepository
    {
        private string _primaryDataBaseConnetion { get; }
        private readonly ILogger<ContadorPedidosTransbordadosRepository> _logger;

        public ContadorPedidosTransbordadosRepository(IOptions<DatabaseConfig> config, ILogger<ContadorPedidosTransbordadosRepository> logger)
        {
            _primaryDataBaseConnetion = config.Value.PrimaryDataBase;
            _logger = logger;
        }

        public async Task UpdateAsync(DateTime data, int entidadeId, ContadorPedidosTransbordadosEntity entity)
        {
            using SqlConnection conn = new SqlConnection(_primaryDataBaseConnetion);
            conn.Open();
            using (SqlTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    var resultQuery = await conn.ExecuteAsync("UPDATE TransbordoMesa.ContadorPedidosTransbordados SET PedidosNaoFinalizados =PedidosNaoFinalizados+ @pedidosNaoFinalizados, PedidosTransbordados =PedidosTransbordados+ @pedidosTransbordados where Data = @data and EntidadeId = @entidadeId", new { data = data, entidadeId = entity.EntidadeId, pedidosNaoFinalizados = entity.PedidosNaoFinalizados, pedidosTransbordados = entity.PedidosTransbordados }, transaction);
                    if (resultQuery == 0)
                        await conn.ExecuteAsync("insert into TransbordoMesa.ContadorPedidosTransbordados " +
                            "(Data, EntidadeId, PedidosNaoFinalizados, PedidosTransbordados) values (@data, @entidadeId," +
                            " @pedidosNaoFinalizados, @pedidosTransbordados)", new { data = data, entidadeId = 
                            entity.EntidadeId, pedidosNaoFinalizados = entity.PedidosNaoFinalizados, pedidosTransbordados = entity.PedidosTransbordados },
                            transaction);
                    
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro na alteração do registro na tabela TransbordoMesa.ContadorPedidosTransbordados. {@Information}", new { entity });
                    transaction.Rollback();
                }
            }
        }
    }
}
