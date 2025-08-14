using GamePlatform.Services.Contracts;
using StackExchange.Redis;

namespace GamePlatform.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly IRedisService _redis;
    private const string LeaderboardKey = "leaderboard";

    public LeaderboardService(IRedisService redis) => _redis = redis;

    public async Task AddScoreAsync(string playerId, int points)
    {
        await _redis.Db.SortedSetAddAsync(LeaderboardKey, playerId, points);
    }

    public async Task<double?> GetRankAsync(string playerId)
    {
        return await _redis.Db.SortedSetRankAsync(LeaderboardKey, playerId, Order.Descending);
    }

    public async Task<IEnumerable<SortedSetEntry>> GetTopAsync(int count)
    {
        return await _redis.Db.SortedSetRangeByRankWithScoresAsync(LeaderboardKey, 0, count - 1, Order.Descending);
    }
}
