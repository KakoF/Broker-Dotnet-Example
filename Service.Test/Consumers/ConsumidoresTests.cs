using Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client.Events;
using Service.Consumers.Consumers;
using Service.Services;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Service.Test.Consumers
{
    public class ConsumidoresTests
    {
        private IConfiguration _config;

        private readonly MockRepository _mockRepository;
        private readonly Mock<IEntidadeService> _service;
        private readonly Mock<IPedidoStatusService> _pedidoStatusService;
        private readonly Mock<ILogger<ConsumidorStatusAlterado>> _logger;
        private readonly Mock<ILogger<ConsumidorTransbordo>> _loggerTransbordo;

        private readonly ConsumidorStatusAlterado _consumidorStatusAlterado;

        private readonly ConsumidorTransbordo _consumidorTransbordo;

        public ConsumidoresTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Loose);

            _service = _mockRepository.Create<IEntidadeService>();
            _pedidoStatusService = _mockRepository.Create<IPedidoStatusService>();
            _logger = _mockRepository.Create<ILogger<ConsumidorStatusAlterado>>();
            _loggerTransbordo = _mockRepository.Create<ILogger<ConsumidorTransbordo>>();

            _consumidorStatusAlterado = new ConsumidorStatusAlterado(_logger.Object, Configuration, _service.Object, _pedidoStatusService.Object,
                new ComputedPedidos());

            _consumidorTransbordo = new ConsumidorTransbordo(_loggerTransbordo.Object, Configuration, _service.Object,
                new ComputedPedidos());
        }

        [Fact(DisplayName = "Quando consumir fila de status alterado, deverá desserializar mensagem e consumir repositórios e serviços")]
        public async Task Consumer_Received01()
        {
            //Arr
            _service.Setup(c => c.EntidadeConfiguradaParaContador(It.IsAny<int>()))
                .ReturnsAsync(true);

            _pedidoStatusService.Setup(c => c.StatusFinalizacao(It.IsAny<int?>()))
                .ReturnsAsync((false, false));


            var json = "{ EntidadeId: 100515, Sucesso: true, Estado: { StatusId: 1 } }";
            byte[] data = Encoding.UTF8.GetBytes(json);

            //Act
            await _consumidorStatusAlterado.Consumer_Received(null, new BasicDeliverEventArgs { Body = data.AsMemory() });

            //Assert
            _service.Verify(c => c.EntidadeConfiguradaParaContador(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Quando consumir fila de transbordo, deverá desserializar mensagem e consumir repositórios e serviços")]
        public async Task Consumer_Received02()
        {
            //Arr
            _service.Setup(c => c.EntidadeConfiguradaParaContador(It.IsAny<int>()))
                .ReturnsAsync(true);

            var json = "'{ EntidadeId: 100515 }'";
            byte[] data = Encoding.UTF8.GetBytes(json);

            //Act
            await _consumidorTransbordo.Consumer_Received(null, new BasicDeliverEventArgs { Body = data.AsMemory() });

            //Assert
            _service.Verify(c => c.EntidadeConfiguradaParaContador(It.IsAny<int>()), Times.Once);
        }

        public IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.dev-local.json", optional: false);
                    _config = builder.Build();
                }

                return _config;
            }
        }
    }
}
