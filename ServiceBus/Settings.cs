using Microsoft.Extensions.Configuration;
using System;

namespace ServiceBus
{
	public class Settings
	{
		public readonly string HostName;
		public readonly string VirtualHost;
		public readonly string UserName;
		public readonly string Password;
		public readonly ushort MaximoConsumoMensagem;
		public readonly ushort QtdeConsumidores;

		public readonly string Exchange;
		public readonly string Fila;
		public readonly string RoutingKey;


		public Settings(string hostname, string virtualhost, string username, string password, ushort maximoConsumoMensagem, ushort qtdeConsumidores)
		{
			HostName = hostname;
			VirtualHost = virtualhost;
			UserName = username;
			Password = password;
			MaximoConsumoMensagem = maximoConsumoMensagem;
			QtdeConsumidores = qtdeConsumidores;
		}

		public Settings(string nome, IConfiguration config)
		{
			var conexao = config.GetSection($"Rabbit:Connection").Value;

			var parametros = conexao.Split(";");

			foreach (var item in parametros)
			{
				if (string.IsNullOrWhiteSpace(item)) continue;

				var valores = item.Split("=");
				if (valores[0].ToUpper().Trim() == "HOST") HostName = valores[1].Split(",")[0].Trim();
				if (valores[0].ToUpper().Trim() == "VIRTUALHOST") VirtualHost = valores[1].Trim();
				if (valores[0].ToUpper().Trim() == "USERNAME") UserName = valores[1].Trim();
				if (valores[0].ToUpper().Trim() == "PASSWORD") Password = valores[1].Trim();
			}

			MaximoConsumoMensagem = (ushort)Convert.ToInt16(config.GetSection($"Rabbit:MaximoConsumoMensagem").Value);
			QtdeConsumidores = (ushort)Convert.ToInt16(config.GetSection($"Rabbit:QuantidadeConsumidores").Value);
			Exchange = config.GetSection($"Rabbit:{nome}:Exchange").Value;
			Fila = config.GetSection($"Rabbit:{nome}:Fila").Value;
			RoutingKey = config.GetSection($"Rabbit:{nome}:RoutingKey").Value;
		}
	}
}