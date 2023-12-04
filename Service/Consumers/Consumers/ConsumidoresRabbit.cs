using Domain.Interfaces.Consumers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Consumers.Consumers
{
	public class ConsumidoresRabbit : IConsumidoresRabbit
	{
		private readonly IEnumerable<IConsumidor> _consumidores;

		public ConsumidoresRabbit(IEnumerable<IConsumidor> consumidores)
		{
			_consumidores = consumidores;
		}

		public Task IniciarAsync(CancellationToken cancellationToken)
		{
			foreach (IConsumidor item in _consumidores)
			{
				item.Consumir();
			}

			return Task.CompletedTask;
		}
	}
}