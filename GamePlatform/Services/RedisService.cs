using GamePlatform.Services.Contracts;
using StackExchange.Redis;

namespace GamePlatform.Services;

public class RedisService : IRedisService
{
    private readonly ConnectionMultiplexer _redis;

    public IDatabase Db => _redis.GetDatabase();

    public ISubscriber Subscriber => _redis.GetSubscriber();

    public RedisService(IConfiguration config)
    {
        _redis = ConnectionMultiplexer.Connect(config.GetConnectionString("Redis") ?? config["Redis:Connection"]);
    }
}
