namespace GamePlatform.Services;

public class ActivityService
{
    private readonly RedisService _redis;
    private const string DailyLoginPrefix = "activity:daily:";
    private const string UniquePlayersKey = "activity:unique";

    public ActivityService(RedisService redis) => _redis = redis;

    // Track daily login using Bitmap
    public async Task MarkLoginAsync(string playerId)
    {
        var key = DailyLoginPrefix + DateTime.UtcNow.ToString("yyyyMMdd");
        if (!int.TryParse(playerId, out int offset)) offset = playerId.GetHashCode() & int.MaxValue;
        await _redis.Db.StringSetBitAsync(key, offset, true);
    }

    public async Task<long> CountDailyActivePlayersAsync(DateTime date)
    {
        var key = DailyLoginPrefix + date.ToString("yyyyMMdd");
        return await _redis.Db.StringBitCountAsync(key);
    }

    // Track unique players using HyperLogLog
    public async Task AddUniquePlayerAsync(string playerId)
    {
        await _redis.Db.HyperLogLogAddAsync(UniquePlayersKey, playerId);
    }

    public async Task<long> GetUniquePlayerCountAsync()
    {
        return await _redis.Db.HyperLogLogLengthAsync(UniquePlayersKey);
    }
}
