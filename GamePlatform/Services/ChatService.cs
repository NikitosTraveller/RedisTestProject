using GamePlatform.Models;
using GamePlatform.Services.Contracts;

namespace GamePlatform.Services;

public class ChatService : IChatService
{
    private readonly IRedisService _redis;

    public ChatService(IRedisService redis) => _redis = redis;

    public async Task PublishAsync(string channel, Message message)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(message);
        await _redis.Subscriber.PublishAsync(channel, json);
    }

    public void Subscribe(string channel, Action<Message> handler)
    {
        _redis.Subscriber.Subscribe(channel, (chan, val) =>
        {
            var msg = System.Text.Json.JsonSerializer.Deserialize<Message>(val!);
            handler(msg!);
        });
    }
}
