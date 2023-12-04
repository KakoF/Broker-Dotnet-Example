using Core.Interfaces.Resiliencia;
using Domain.Entity;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Infra.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Polly;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Service.Test.Services
{
    public class ContadorPedidosTransbordadosServiceTests
    {
        private MockRepository _mockRepository;
        private Mock<IContadorPedidosTransbordadosRepository> _mockContadorPedidosTransbordadosRepository;
        private Mock<IEntidadeService> _mockEntidadeService;
        private Mock<IAsyncPolicies> _mockAsyncPolicies;
        private Mock<ILogger<ContadorPedidosTransbordadosService>> _mockLogger;
        private IOptions<RetryConfig> _options = Options.Create<RetryConfig>(new RetryConfig());
        private readonly ComputedPedidos _computedPedidos;
        private ContadorPedidosTransbordadosService _sut;

        public ContadorPedidosTransbordadosServiceTests()
        {

            _options = Options.Create(new RetryConfig());

            _mockRepository = new MockRepository(MockBehavior.Loose);
            _mockContadorPedidosTransbordadosRepository = _mockRepository.Create<IContadorPedidosTransbordadosRepository>();
            _mockEntidadeService = _mockRepository.Create<IEntidadeService>();
            _mockAsyncPolicies = _mockRepository.Create<IAsyncPolicies>();
            _mockLogger = _mockRepository.Create<ILogger<ContadorPedidosTransbordadosService>>();
            _computedPedidos = new ComputedPedidos();

            _sut = new ContadorPedidosTransbordadosService(_mockContadorPedidosTransbordadosRepository.Object,
            _mockEntidadeService.Object,
            _mockAsyncPolicies.Object,
            _mockLogger.Object,
            _options,
            _computedPedidos);
        }

        [Fact(DisplayName = "Quando salvar os dados para contagem, deve persistir os dados na base e logs")]
        public async Task SalvarContador()
        {
            //Arr
            var entidadesConfiguradas = new List<EntidadeConfiguradaModel>()
            {
                new EntidadeConfiguradaModel() { EntidadeId = 100515}
            };
            var entity = new ContadorPedidosTransbordadosEntity(100515, 1, 1);
            Random jitterer = new Random();
            IAsyncPolicy asyncPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(1,
                    retryAttempt => TimeSpan.FromMilliseconds(1) + TimeSpan.FromMilliseconds(jitterer.Next(0, 10)),
                    (exception, timeSpan) =>
                    { });

            _mockEntidadeService.Setup(x => x.GetEntidadesConfiguradasAsync()).ReturnsAsync(entidadesConfiguradas);

            _computedPedidos.IncrementarPedidosFinalizados(100515);
            _computedPedidos.IncrementarPedidosTransbordados(100515);

            _mockContadorPedidosTransbordadosRepository.Setup(x => x.UpdateAsync(It.IsAny<DateTime>(), 100515, entity));

            _mockAsyncPolicies.Setup(x => x.GerarRetryPolicy(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<TimeSpan>())).Returns(asyncPolicy);

            //Act
            await _sut.Salvar();

            //Assert

            _mockEntidadeService.
               Verify(x => x.GetEntidadesConfiguradasAsync(), Times.Once);

            _mockAsyncPolicies.
                Verify(x => x.GerarRetryPolicy(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<TimeSpan>()), Times.Once);
        }
    }
}
