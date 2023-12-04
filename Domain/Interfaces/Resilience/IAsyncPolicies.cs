using Polly;
using System;

namespace Core.Interfaces.Resiliencia
{
    public interface IAsyncPolicies
    {
        IAsyncPolicy GerarRetryPolicy(int retryCount, string metodo, TimeSpan time);
    }
}
