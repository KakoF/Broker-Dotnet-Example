using Domain.Entity;
using Infra.Cache;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace Infra.Test.Cache
{
    public class MemoryCacheTest
    {
        private readonly MockRepository _mock;
        private readonly MemoryCache _sut;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        

        public MemoryCacheTest()
        {
            _mock = new MockRepository(MockBehavior.Loose);
            _mockMemoryCache = _mock.Create<IMemoryCache>();
            _sut = new MemoryCache(_mockMemoryCache.Object);
        }

        
        [Fact(DisplayName = "Remove Cache de Objeto")]
        public void DeveRemoverCache_ComSucesso()
        {
            // Arrange
            int id = It.IsAny<int>();
            ContadorPedidosTransbordadosEntity data = new ContadorPedidosTransbordadosEntity()
            {
                EntidadeId = It.IsAny<int>(),
                PedidosNaoFinalizados = It.IsAny<int>(),
                PedidosTransbordados = It.IsAny<int>()
            };

            _mockMemoryCache.
               Setup(x => x.Remove(It.IsAny<string>()));

            // Act
            _sut.Remove($"CacheContator:{id}");

            // Assert
            _mockMemoryCache.
              Verify(x => x.Remove(It.IsAny<string>()), Times.Once);

        }
    }
}
