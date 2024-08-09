using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace HackerNewsViewer.WebAPI.Services
{
    public interface ICacheService
    {
        public T AddItem<T>(string key, T value);
        public T GetItem<T>(string key);
        public void RemoveItem(string key);
    }

    internal class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _entryOptions;

        public CacheService(IMemoryCache cache, 
            int absoluteExpirationRelativeToNowMinutes, 
            int slidingExpirationMinutes)
        {
            _cache = cache;
            _entryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(absoluteExpirationRelativeToNowMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpirationMinutes)
            };
        }

        public T AddItem<T>(string key, T value)
        {
            return _cache.Set(key, value, _entryOptions);
        }

        public T GetItem<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public void RemoveItem(string key)
        {
            _cache.Remove(key);
        }
    }
}
