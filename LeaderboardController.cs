using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ContestSystem.Interface;

namespace ContestSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardRepository _repository;

        public LeaderboardController(ILeaderboardRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{contestId}")]
        public async Task<IActionResult> Get(Guid contestId)
        {
            var leaderboard = await _repository.GetLeaderboardAsync(contestId);

            return Ok(leaderboard.Select(l => new
            {
                Rank = l.Rank,
                Email = l.Email,
                Score = l.Score
            }));
        }
    }
}