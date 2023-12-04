using System;
using System.Threading.Tasks;

namespace Domain.Interfaces.Cache
{
    public interface ICache
    {
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> function, int? minutesToExpire = null);
        void Remove(string key);
    }
}
