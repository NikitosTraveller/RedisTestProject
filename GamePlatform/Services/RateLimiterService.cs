using GamePlatform.Services.Contracts;

namespace GamePlatform.Services;

public class RateLimiterService : IRateLimiterService
{
    private readonly RedisManager _redis;

    public RateLimiterService(RedisManager redis) => _redis = redis;

    public async Task<bool> IsAllowedAsync(string key, int limit, TimeSpan window)
    {
        var current = await _redis.Db.StringIncrementAsync(key);
        if (current == 1) await _redis.Db.KeyExpireAsync(key, window);
        return current <= limit;
    }
}
