using System.Diagnostics.CodeAnalysis;

namespace Infra.Config
{
    [ExcludeFromCodeCoverage]
    public class RetryConfig
    {
        public int Quantidade { get; set; }
        public int Intervalo { get; set; }
    }
}
