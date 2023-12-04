using System.Diagnostics.CodeAnalysis;

namespace Infra.Config
{
    [ExcludeFromCodeCoverage]
    public class DatabaseConfig
    {
        public string PrimaryDataBase { get; set; }
        public string SecondaryDatabase { get; set; }
        public string Log { get; set; }
        public string Redis { get; set; }
        public int RedisPoolSize { get; set; }
    }
}
