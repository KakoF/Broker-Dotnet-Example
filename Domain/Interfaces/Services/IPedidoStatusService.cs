using System;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
	public interface IPedidoStatusService
	{
		Task<(bool, bool)> StatusFinalizacao(int? atributo);
	}
}
