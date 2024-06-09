using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Service.Repositories
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Redis");
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _db = _redis.GetDatabase();
        }

        public async Task SetStringAsync(string key, string value, TimeSpan expiry)
        {
            await _db.StringSetAsync(key, value, expiry);
        }

        public async Task<string> GetStringAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public async Task<bool> DeleteKeyAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }
    }
}
