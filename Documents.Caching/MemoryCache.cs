using Documents;
using System.Runtime.Caching;

namespace Documents.Caching
{
    public class MemoryCache : ICache
    {
        private readonly ObjectCache _cache;

        public MemoryCache(ObjectCache memoryCache)
        {
            _cache = memoryCache;
        }

        public void Add(string key, Document value, DateTimeOffset expirationTime)
        {
            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = expirationTime };
            _cache.AddOrGetExisting(key, value, policy);
        }

        public bool TryGetValue(string key, out Document value)
        {
            value = _cache.Get(key) as Document;
            return value != null;
        }
    }
}