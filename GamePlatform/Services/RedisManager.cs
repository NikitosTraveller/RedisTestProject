using StackExchange.Redis;

namespace GamePlatform.Services;

public class RedisManager
{
    private readonly ConnectionMultiplexer _redis;

    public IDatabase Db => _redis.GetDatabase();

    public ISubscriber Subscriber => _redis.GetSubscriber();

    public RedisManager(IConfiguration config)
    {
        _redis = ConnectionMultiplexer.Connect(config.GetConnectionString("Redis") ?? config["Redis:Connection"]);
    }
}
