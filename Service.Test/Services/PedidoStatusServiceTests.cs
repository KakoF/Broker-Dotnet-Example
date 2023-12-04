using Domain.Entities;
using Domain.Interfaces.Cache;
using Domain.Interfaces.Repositories;
using Moq;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Service.Test.Services
{
    public class PedidoStatusServiceTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IPedidoStatusRepository> _mockPedidoStatusRepository;
        private readonly Mock<ICache> _mockCache;

        private readonly PedidoStatusService _pedidoStatusService;

        public PedidoStatusServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Loose);

            _mockPedidoStatusRepository = _mockRepository.Create<IPedidoStatusRepository>();
            _mockCache = _mockRepository.Create<ICache>();

            _pedidoStatusService = new PedidoStatusService(_mockPedidoStatusRepository.Object, _mockCache.Object);
        }

        [Theory(DisplayName = "Quando buscar status finalizado de pedidos, deverá comparar e validar parâmetro de status enviado")]
        [InlineData(1)]
        public async Task StatusFinalizado(int? status)
        {
            //Arr
            var list = new List<PedidosStatusEntity>() {
                    new PedidosStatusEntity() { Atributos = 1, PedidoStatusId = 1 },
                    new PedidosStatusEntity() { Atributos = 2, PedidoStatusId = 2 },
                    new PedidosStatusEntity() { Atributos = 3, PedidoStatusId = 3 }
                };

            _mockPedidoStatusRepository.Setup(c => c.ObterStatusDeFinalizacao()).ReturnsAsync(list);
            _mockCache.Setup(c => c.GetOrCreateAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<PedidosStatusEntity>>>>(), It.IsAny<int?>())).ReturnsAsync(list);

            //Act
            var result = await _pedidoStatusService.StatusFinalizacao(status);

            //Assert
            Assert.True(result.Item1);
            Assert.False(result.Item2);
        }
    }
}
