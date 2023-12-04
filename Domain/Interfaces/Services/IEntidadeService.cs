using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
	public interface IEntidadeService
	{
		Task<IEnumerable<EntidadeConfiguradaModel>> GetEntidadesConfiguradasAsync();

		Task<bool> EntidadeConfiguradaParaContador(int entidadeId);
	}
}
