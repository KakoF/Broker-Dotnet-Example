using System.Collections.Concurrent;

namespace Service.Services
{
    public class ComputedPedidos
    {
        private static readonly object _lockPedidosFinalizados = new object();
        private static readonly object _lockPedidosTransbordados = new object();
        public int PedidosComputadosFinalizados { get; set; }
        public int PedidosComputadosTransbordados { get; set; }
        public ConcurrentDictionary<int, int> PedidosFinalizados { get; set; }
        public ConcurrentDictionary<int, int> PedidosTransbordados { get; set; }
        public ComputedPedidos()
        {
            PedidosFinalizados = new ConcurrentDictionary<int, int>();
            PedidosTransbordados = new ConcurrentDictionary<int, int>();
            PedidosComputadosFinalizados = 0;
            PedidosComputadosTransbordados = 0;
        }

        public void IncrementarPedidosFinalizados(int entidadeId)
        {
            lock (_lockPedidosFinalizados)
            {
                PedidosFinalizados.AddOrUpdate(entidadeId, 1, (key, oldTuple) => oldTuple + 1);
                PedidosComputadosFinalizados++;
            }
        }

        public void DecrementarPedidosFinalizados(int entidadeId)
        {
            lock (_lockPedidosFinalizados)
            {
                PedidosFinalizados.AddOrUpdate(entidadeId, -1, (key, oldTuple) => oldTuple - 1);
                PedidosComputadosFinalizados--;
            }
        }

        public void IncrementarPedidosTransbordados(int entidadeId)
        {
            lock (_lockPedidosTransbordados)
            {
                PedidosTransbordados.AddOrUpdate(entidadeId, 1, (key, oldTuple) => oldTuple + 1);
                PedidosComputadosTransbordados++;
            }
        }
        
        public int BuscarPedidosFinalizados(int entidadeId)
        {
            PedidosFinalizados.TryGetValue(entidadeId, out int quantidade);
            return quantidade;
        }

        public int BuscarPedidosTransbordados(int entidadeId)
        {
            PedidosTransbordados.TryGetValue(entidadeId, out int quantidade);
            return quantidade;
        }

        public void RemoverPedidosFinalizados(int entidadeId, int quantidade)
        {
            lock (_lockPedidosFinalizados)
            {
                PedidosFinalizados.AddOrUpdate(entidadeId, 0, (key, oldTuple) => oldTuple - quantidade);
                PedidosComputadosFinalizados -= quantidade;
            }
        }

        public void RemoverPedidosTransbordados(int entidadeId, int quantidade)
        {
            lock (_lockPedidosTransbordados)
            {
                PedidosTransbordados.AddOrUpdate(entidadeId, 0, (key, oldTuple) => oldTuple - quantidade);
                PedidosComputadosTransbordados -= quantidade;
            }
        }
    }
}
