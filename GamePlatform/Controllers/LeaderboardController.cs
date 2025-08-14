using GamePlatform.Models;
using GamePlatform.Services;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaderboardController : ControllerBase
{
    private readonly LeaderboardService _service;
    private readonly RateLimiterService _rateLimiter;

    public LeaderboardController(LeaderboardService service, RateLimiterService rateLimiter)
    {
        _service = service;
        _rateLimiter = rateLimiter;
    }

    [HttpPost("score")]
    public async Task<IActionResult> SubmitScore([FromBody] Score score)
    {
        if (!await _rateLimiter.IsAllowedAsync($"score:{score.PlayerId}", 5, TimeSpan.FromSeconds(10)))
            return TooManyRequests("Rate limit exceeded.");

        await _service.AddScoreAsync(score.PlayerId, score.Points);
        return Ok();
    }

    [HttpGet("top/{count}")]
    public async Task<IActionResult> GetTop(int count)
    {
        var top = await _service.GetTopAsync(count);
        return Ok(top.Select(e => new { PlayerId = e.Element, Score = e.Score }));
    }

    [HttpGet("rank/{playerId}")]
    public async Task<IActionResult> GetRank(string playerId)
    {
        var rank = await _service.GetRankAsync(playerId);
        return Ok(new { PlayerId = playerId, Rank = rank });
    }

    private IActionResult TooManyRequests(string message) => StatusCode(429, new { message });
}
