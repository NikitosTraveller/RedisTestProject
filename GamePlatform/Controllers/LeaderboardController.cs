using GamePlatform.Models;
using GamePlatform.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboard;
    private readonly IRateLimiterService _rateLimiter;

    public LeaderboardController(
        ILeaderboardService service, 
        IRateLimiterService rateLimiter)
    {
        _leaderboard = service;
        _rateLimiter = rateLimiter;
    }

    [HttpPost("score")]
    public async Task<IActionResult> SubmitScore([FromBody] Score score)
    {
        if (!await _rateLimiter.IsAllowedAsync($"score:{score.PlayerId}", 5, TimeSpan.FromSeconds(10)))
            return TooManyRequests("Rate limit exceeded.");

        await _leaderboard.AddScoreAsync(score.PlayerId, score.Points);
        return Ok();
    }

    [HttpGet("top/{count}")]
    public async Task<IActionResult> GetTop(int count)
    {
        var top = await _leaderboard.GetTopAsync(count);
        return Ok(top.Select(e => new { PlayerId = e.Element, Score = e.Score }));
    }

    [HttpGet("rank/{playerId}")]
    public async Task<IActionResult> GetRank(string playerId)
    {
        var rank = await _leaderboard.GetRankAsync(playerId);
        return Ok(new { PlayerId = playerId, Rank = rank });
    }

    private IActionResult TooManyRequests(string message) => StatusCode(429, new { message });
}
