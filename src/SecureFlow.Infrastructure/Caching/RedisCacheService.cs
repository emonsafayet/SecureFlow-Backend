using System.Text.Json;
using StackExchange.Redis;
using SecureFlow.Application.Common.Interfaces;

namespace SecureFlow.Infrastructure.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _db;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);

        // 1️ Always set value (NO expiry here)
        await _db.StringSetAsync(key, json);

        // 2️ Apply expiry separately (THIS is the fix)
        if (expiry.HasValue && expiry.Value > TimeSpan.Zero)
        {
            await _db.KeyExpireAsync(key, expiry.Value);
        }
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);

        if (!value.HasValue)
            return default;

        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }
}
