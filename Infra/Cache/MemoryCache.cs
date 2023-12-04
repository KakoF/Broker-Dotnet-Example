using Domain.Interfaces.Cache;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Infra.Cache
{
    public class MemoryCache : ICache
    {
        const string CACHE_APP_KEY = "ContadorTransbordo:";
        const int EXPIRATION_MINUTES = 30;
        private readonly IMemoryCache _memoryCache;

        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> function, int? minutesToExpire = null)
        {
            var data = await _memoryCache.GetOrCreateAsync<T>($"{CACHE_APP_KEY}{key}", async entry => {

                entry.SlidingExpiration = TimeSpan.FromMinutes(minutesToExpire ?? EXPIRATION_MINUTES);
                return await function();
            });
            return data;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove($"{CACHE_APP_KEY}{key}");
        }
    }
}