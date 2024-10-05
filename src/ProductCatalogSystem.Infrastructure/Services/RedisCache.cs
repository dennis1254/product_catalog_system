
using ProductCatalogSystem.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace ProductCatalogSystem.Infrastructure.Services
{

    public class RedisCache : ICache
    {
        private readonly IDatabase _redis;

        public RedisCache(IConfiguration config)
        {
            var connectionMultiplexer = ConnectionMultiplexer.Connect(config["redis:host"]);
            _redis = connectionMultiplexer.GetDatabase(0);
        }

        
        public async Task<string> GetString(string key, string collection = "default")
        {
            return await _redis.StringGetAsync(key);
        }

        public async Task<bool> RemoveString(string key, string collection = "default")
        {
            return await _redis.KeyDeleteAsync(key);
        }

        public async Task<bool> SetString(string key, string data, string collection = "default", long expirationInMin = 1440)
        {
            return await _redis.StringSetAsync(key, data,TimeSpan.FromMinutes(expirationInMin));
        }

    }
}
