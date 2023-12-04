using Core.Interfaces.Resiliencia;
using Microsoft.Extensions.Logging;
using Polly;
using System;

namespace Domain.Resilience
{
    public class AsyncPolicies : IAsyncPolicies
    {
        private readonly ILogger<AsyncPolicies> _logger;
        private readonly Random _jitterer = new Random();
        public AsyncPolicies(ILogger<AsyncPolicies> logger)
        {
            _logger = logger;
        }

        public IAsyncPolicy GerarRetryPolicy(int retryCount, string metodo, TimeSpan time)
        {
            return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                    retryCount,
                    retryAttempt => time + TimeSpan.FromMilliseconds(_jitterer.Next(0, 10)),
                    (exception, timeSpan) =>
                    {
                        _logger.LogError(exception, $"Erro na chamada do método {metodo}");
                    });
        }
    }
}