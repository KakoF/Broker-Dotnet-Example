using System;

namespace Domain.Models
{
    public class StatusAlteradoMessageModel
    {
        public long ID { get; set; }
        public int EntidadeId { get; set; }
        public string PedidoCodigoEntidade { get; set; }
        public DateTime Data { get; set; }
        public string Tipo { get; set; }
        public int StatusAtual { get; set; }
        public int? AtributosStatusAtual { get; set; }
        public int StatusNovo { get; set; }
        public int? AtributosStatusNovo { get; set; }

        private bool ErroOuIntegracao
        {
            get { return StatusNovo == -4 || StatusNovo == -6 || StatusNovo == -1; }
        }

        public bool StatusNovoNaoFinalizado
        {
            get { return StatusNovo == 3 || StatusNovo == 225 || StatusNovo == 171; }
        }


        private bool StatusAtualNaoFinalizado
        {
            get { return StatusAtual == 3 || StatusAtual == 225 || StatusAtual == 171; }
        }

        public bool Incrementar()
        {
            if (StatusNovoNaoFinalizado && !StatusAtualNaoFinalizado)
                return true;

            return false;

        }

        public bool Decrementar(bool Automatico, bool Finalizado)
        {
            if(StatusNovoErroOuIntegracao())
                return true;

            if (Finalizado && Automatico)
            {
                if (StatusAtualNaoFinalizado)
                    return true;
            }
            else
            {
                if (!Finalizado && StatusAtualNaoFinalizado)
                    return true;
            }
            return false;

        }

        private bool StatusNovoErroOuIntegracao()
        {
            return ErroOuIntegracao && StatusAtualNaoFinalizado;
        }
    }

}
