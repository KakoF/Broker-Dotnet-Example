using Service.Services;
using System.Linq;
using Xunit;

namespace Service.Test.Services
{
    public class ComputedPedidosTests
    {
        private readonly ComputedPedidos _computedPedidos;

        public ComputedPedidosTests()
        {
            _computedPedidos = new ComputedPedidos();
        }

        [Theory(DisplayName = "Quando contabilizar pedidos finalizados, estes deverão ser computados")]
        [InlineData(3, 100515)]
        public void IncrementarPedidosFinalizados(int qtdPedidos, int entidadeId)
        {
            //Act
            for (int i = 0; i < qtdPedidos; i++)
                _computedPedidos.IncrementarPedidosFinalizados(entidadeId);

            //Assert
            Assert.Single(_computedPedidos.PedidosFinalizados);
            Assert.Equal(qtdPedidos, _computedPedidos.PedidosFinalizados.Values.First());
            Assert.Equal(qtdPedidos, _computedPedidos.PedidosComputadosFinalizados);
        }

        [Theory(DisplayName = "Quando contabilizar pedidos transbordados, estes deverão ser computados")]
        [InlineData(3, 100515)]
        public void IncrementarPedidosTransbordados(int qtdPedidos, int entidadeId)
        {
            //Act
            for (int i = 0; i < qtdPedidos; i++)
                _computedPedidos.IncrementarPedidosTransbordados(entidadeId);

            //Assert
            Assert.Single(_computedPedidos.PedidosTransbordados);
            Assert.Equal(qtdPedidos, _computedPedidos.PedidosTransbordados.Values.First());
            Assert.Equal(qtdPedidos, _computedPedidos.PedidosComputadosTransbordados);
        }

        [Theory(DisplayName = "Quando buscar quantidade de pedidos finalizados, deverá retornar quantidade de pedidos contabilizados")]
        [InlineData(3, 100515)]
        public void BuscarPedidosFinalizados(int qtdPedidos, int entidadeId)
        {
            //Arr
            for (int i = 0; i < qtdPedidos; i++)
                _computedPedidos.IncrementarPedidosFinalizados(entidadeId);

            //Act
            var result = _computedPedidos.BuscarPedidosFinalizados(entidadeId);

            //Assert
            Assert.Single(_computedPedidos.PedidosFinalizados);
            Assert.Equal(qtdPedidos, _computedPedidos.PedidosFinalizados.Values.First());
            Assert.Equal(qtdPedidos, _computedPedidos.PedidosComputadosFinalizados);
            Assert.Equal(qtdPedidos, result);
        }

        [Theory(DisplayName = "Quando buscar quantidade de pedidos transbordados, deverá retornar quantidade de pedidos contabilizados")]
        [InlineData(3, 100515)]
        public void BuscarPedidosTransbordados(int qtdPedidos, int entidadeId)
        {
            //Arr
            for (int i = 0; i < qtdPedidos; i++)
                _computedPedidos.IncrementarPedidosTransbordados(entidadeId);

            //Act
            var result = _computedPedidos.BuscarPedidosTransbordados(entidadeId);

            //Assert
            Assert.Single(_computedPedidos.PedidosTransbordados);
            Assert.Equal(qtdPedidos, _computedPedidos.PedidosTransbordados.Values.First());
            Assert.Equal(qtdPedidos, _computedPedidos.PedidosComputadosTransbordados);
            Assert.Equal(qtdPedidos, result);
        }

        [Theory(DisplayName = "Quando remover pedidos finalizados, estes deverão ser computados")]
        [InlineData(3, 1, 100515)]
        public void RemoverPedidosFinalizados(int qtdPedidos, int qtdPedidosParaRemover, int entidadeId)
        {
            //Arr
            for (int i = 0; i < qtdPedidos; i++)
                _computedPedidos.IncrementarPedidosFinalizados(entidadeId);

            //Act
            _computedPedidos.RemoverPedidosFinalizados(entidadeId, qtdPedidosParaRemover);

            //Assert
            Assert.Single(_computedPedidos.PedidosFinalizados);
            Assert.Equal(qtdPedidos - qtdPedidosParaRemover, _computedPedidos.PedidosFinalizados.Values.First());
            Assert.Equal(qtdPedidos - qtdPedidosParaRemover, _computedPedidos.PedidosComputadosFinalizados);
        }

        [Theory(DisplayName = "Quando remover pedidos transbordados, estes deverão ser computados")]
        [InlineData(3, 1, 100515)]
        public void RemoverPedidosTransbordados(int qtdPedidos, int qtdPedidosParaRemover, int entidadeId)
        {
            //Arr
            for (int i = 0; i < qtdPedidos; i++)
                _computedPedidos.IncrementarPedidosTransbordados(entidadeId);

            //Act
            _computedPedidos.RemoverPedidosTransbordados(entidadeId, qtdPedidosParaRemover);

            //Assert
            Assert.Single(_computedPedidos.PedidosTransbordados);
            Assert.Equal(qtdPedidos - qtdPedidosParaRemover, _computedPedidos.PedidosTransbordados.Values.First());
            Assert.Equal(qtdPedidos - qtdPedidosParaRemover, _computedPedidos.PedidosComputadosTransbordados);
        }
    }
}
