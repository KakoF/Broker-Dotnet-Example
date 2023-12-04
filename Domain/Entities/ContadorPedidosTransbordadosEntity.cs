namespace Domain.Entity
{
    public class ContadorPedidosTransbordadosEntity
    {
        public int EntidadeId { get; set; }
        public int PedidosNaoFinalizados { get; set; }
        public int PedidosTransbordados { get; set; }

        public ContadorPedidosTransbordadosEntity()
        {

        }

        public ContadorPedidosTransbordadosEntity(int entidadeId, int pedidosNaoFinalizados, int pedidosTransbordados)
        {
            EntidadeId = entidadeId;
            PedidosNaoFinalizados = pedidosNaoFinalizados;
            PedidosTransbordados = pedidosTransbordados;
        }
    }

}
