using Application;
using Core.Interfaces.Resiliencia;
using Domain.Interfaces.Cache;
using Domain.Interfaces.Consumers;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Resilience;
using Infra.Cache;
using Infra.Config;
using Infra.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using Service.Consumers.Consumers;
using Service.Services;


IHost host = Host.CreateDefaultBuilder(args)
                .UseNLog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<AsyncPolicies>();
                    services.AddHostedService<Worker>();
                    services.AddMemoryCache();
                    services.AddSingleton<ComputedPedidos>();
                    services.AddSingleton<ICache, MemoryCache>();
                    services.AddSingleton<IAsyncPolicies, AsyncPolicies>();
                    services.AddSingleton<IConsumidoresRabbit, ConsumidoresRabbit>();
                    services.AddSingleton<IConsumidor, ConsumidorStatusAlterado>();
                    services.AddSingleton<IConsumidor, ConsumidorTransbordo>();
                    services.AddSingleton<IEntidadeService, EntidadeService>();
                    services.AddSingleton<IPedidoStatusService, PedidoStatusService>();
                    services.AddSingleton<IContadorPedidosTransbordadosService, ContadorPedidosTransbordadosService>();
                    services.AddSingleton<IPedidoStatusRepository, PedidoStatusRepository>();
                    services.AddSingleton<IContadorPedidosTransbordadosRepository, ContadorPedidosTransbordadosRepository>();
                    services.AddSingleton<IEntidadeMetodoRepository, EntidadeMetodoRepository>();
                    services.Configure<DatabaseConfig>(hostContext.Configuration.GetSection("ConnectionStrings"));
                    services.Configure<RetryConfig>(hostContext.Configuration.GetSection("ConfiguracaoRetentativas"));
                    services.Configure<FlushConfig>(hostContext.Configuration.GetSection("ConfiguracaoFlush"));
                })
                .UseWindowsService()
                .Build();

await host.RunAsync();
