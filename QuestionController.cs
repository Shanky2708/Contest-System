using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ContestSystem.Dto;
using ContestSystem.Entity;
using ContestSystem.Interface;
using ContestSystem.Enum;

namespace ContestSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _repository;

        public QuestionController(IQuestionRepository repository)
        {
            _repository = repository;
        }

        // Admin only
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateQuestionDto dto)
        {
            var question = new Question
            {
                //ContestId = dto.ContestId,
                Text = dto.Text,
                //Type = dto.Type,
                Options = dto.Options.Select(o => new Option
                {
                    Text = o.Text,
                    IsCorrect = o.IsCorrect
                }).ToList()
            };

            var id = await _repository.AddQuestionAsync(question);

            return Ok(new { QuestionId = id });
        }

        // Authenticated users can view
        [Authorize]
        [HttpGet("{contestId}")]
        public async Task<IActionResult> GetByContest(Guid contestId)
        {
            var questions = await _repository.GetByContestIdAsync(contestId);

            // IMPORTANT: Hide correct answers
            var result = questions.Select(q => new
            {
                q.Id,
                q.Text,
                q.Type,
                Options = q.Options.Select(o => new
                {
                    o.Id,
                    o.Text
                })
            });

            return Ok(result);
        }
    }
}