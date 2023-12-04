using Dapper;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Infra.Config;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Infra.Repositories
{
    [ExcludeFromCodeCoverage]
    public class EntidadeMetodoRepository : IEntidadeMetodoRepository
    {
        private readonly string _secondaryDatabaseConnetion;

        public EntidadeMetodoRepository(IOptions<DatabaseConfig> config)
        {
            _secondaryDatabaseConnetion = config.Value.SecondaryDatabase;
        }
        public async Task<IEnumerable<EntidadeConfiguradaModel>> ObterEntidadesConfiguradasTransbordoAsync()
        {
            using var dbConnection = new SqlConnection(_secondaryDatabaseConnetion);
            var data = await dbConnection.QueryAsync<EntidadeConfiguradaModel>(@"select ec.EntidadeID from PosMotor.EntidadeConfiguracao ec
                inner join PosMotor.Template t on t.ID = ec.TemplateID
                inner join PosMotor.TemplateMetodo tm on tm.TemplateID = t.ID
                where ec.Ativo = 1 and (tm.MetodoID = 2 and tm.Ativo = 1)");
            return data;

        }

    }
}