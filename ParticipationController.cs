using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ContestSystem.Dto;
using ContestSystem.Interface;

namespace ContestSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ParticipationController : ControllerBase
    {
        private readonly IParticipationRepository _repository;

        public ParticipationController(IParticipationRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit(SubmitContestDto dto)
        {
            var userId = Guid.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (await _repository.HasParticipatedAsync(dto.ContestId, userId))
                return BadRequest(new { message = "Already participated." });

            var formattedAnswers = dto.Answers
                .Select(a => (a.QuestionId, a.SelectedOptionIds))
                .ToList();

            var score = await _repository.SubmitContestAsync(
                dto.ContestId,
                userId,
                formattedAnswers);

            return Ok(new { Score = score });
        }
    }
}