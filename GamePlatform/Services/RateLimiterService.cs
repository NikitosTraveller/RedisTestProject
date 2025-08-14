namespace GamePlatform.Services;

public class RateLimiterService
{
    private readonly RedisService _redis;

    public RateLimiterService(RedisService redis) => _redis = redis;

    public async Task<bool> IsAllowedAsync(string key, int limit, TimeSpan window)
    {
        var current = await _redis.Db.StringIncrementAsync(key);
        if (current == 1) await _redis.Db.KeyExpireAsync(key, window);
        return current <= limit;
    }
}
