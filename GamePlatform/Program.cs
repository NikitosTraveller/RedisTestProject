
using GamePlatform.Services;

namespace GamePlatform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<RedisService>();
            builder.Services.AddSingleton<LeaderboardService>();
            builder.Services.AddSingleton<RateLimiterService>();
            builder.Services.AddSingleton<ChatService>();
            builder.Services.AddSingleton<PlayerService>();
            builder.Services.AddSingleton<ActivityService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapControllers();
            app.Run();
        }
    }
}
