using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces.Consumers
{
	public interface IConsumidoresRabbit
	{
		Task IniciarAsync(CancellationToken cancellationToken);
	}
}
