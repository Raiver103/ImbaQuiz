using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using news_service.API.Interfaces;

namespace news_service.API.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        private readonly IMemoryCache _memoryCache;

        private const string MemoryCacheKey = "news_cache";
        private readonly TimeSpan _memoryCacheDuration = TimeSpan.FromSeconds(30);  

        public NewsRepository(IConnectionMultiplexer redis, IMemoryCache memoryCache)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
            _memoryCache = memoryCache;
        }

        public async Task<string> AddNewsAsync(string news)
        {
            await _db.ListLeftPushAsync("news", news);
            _memoryCache.Remove(MemoryCacheKey); 
            return news;
        }

        public async Task<IEnumerable<string>> GetNewsAsync()
        { 
            if (_memoryCache.TryGetValue(MemoryCacheKey, out IEnumerable<string> cachedNews))
            {
                return cachedNews;
            }
 
            var newsList = await _db.ListRangeAsync("news", 0, -1);
            var result = newsList.Select(n => n.ToString()).ToList();
 
            _memoryCache.Set(MemoryCacheKey, result, _memoryCacheDuration);

            return result;
        }
    }
}
