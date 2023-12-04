using System.Diagnostics.CodeAnalysis;

namespace Infra.Config
{
    [ExcludeFromCodeCoverage]
    public class FlushConfig
    {
        public int QuantidadeMaxima { get; set; }
        public long Intervalo { get; set; }
       
    }
}
