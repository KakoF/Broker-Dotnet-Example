using System;
namespace Domain.Interfaces.Consumers
{
	public interface IConsumidor : IDisposable
	{
		void Consumir();
	}
}
