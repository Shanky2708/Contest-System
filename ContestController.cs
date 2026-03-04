using ContestSystem.Entity;
using ContestSystem.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using ContestSystem.Enum;
using ContestSystem.Dto;

namespace ContestSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContestController : ControllerBase
    {
        private readonly IContestRepository _repository;
        private readonly ILogger<ContestController> _logger;

        public ContestController(IContestRepository repository,
                                 ILogger<ContestController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: api/contest
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contests = await _repository.GetAllAsync();

            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (userRole == "Normal")
            {
                contests = contests
                    .Where(c => c.AccessLevel == AccessLevel.Normal)
                    .ToList();
            }

            return Ok(contests);
        }

        // GET: api/contest/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var contest = await _repository.GetByIdAsync(id);

            if (contest == null)
            {
                _logger.LogWarning("Contest not found with Id: {ContestId}", id);
                return NotFound(new { message = "Contest not found" });
            }

            return Ok(contest);
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateContest(CreateContestDto dto)
        {
            var contestId = await _repository.CreateContestAsync(dto);
            return Ok(new { ContestId = contestId });
        }
        // PUT: api/contest/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Contest contest)
        {
            if (id != contest.Id)
                return BadRequest(new { message = "Id mismatch" });

            var updated = await _repository.UpdateAsync(contest);

            if (!updated)
                return NotFound(new { message = "Contest not found" });

            return Ok(new { message = "Contest updated successfully" });
        }

        // DELETE: api/contest/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _repository.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Contest not found" });

            return Ok(new { message = "Contest deleted successfully" });
        }

        
    }
}