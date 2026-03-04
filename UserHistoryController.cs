using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ContestSystem.Interface;

namespace ContestSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserHistoryController : ControllerBase
    {
        private readonly IUserHistoryRepository _repository;

        public UserHistoryController(IUserHistoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyHistory()
        {
            var userId = Guid.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var history = await _repository.GetUserHistoryAsync(userId);

            var result = history.Select(h => new
            {
                ContestName = h.Contest.Name,
                h.Score,
                h.IsCompleted,
                h.SubmittedAt,
                PrizeWon = h.Score > 0 ? h.Contest.Prize : null
            });

            return Ok(result);
        }
    }
}