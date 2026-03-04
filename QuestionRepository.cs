using Microsoft.EntityFrameworkCore;
using ContestSystem.Entity;
using ContestSystem.Interface;

namespace ContestSystem.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<QuestionRepository> _logger;

        public QuestionRepository(AppDbContext context,
                                  ILogger<QuestionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> AddQuestionAsync(Question question)
        {
            question.Id = Guid.NewGuid();
            question.CreatedAt = DateTime.UtcNow;

            foreach (var option in question.Options)
            {
                option.Id = Guid.NewGuid();
                option.CreatedAt = DateTime.UtcNow;
            }

            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Question created for Contest {ContestId}", question.ContestId);

            return question.Id;
        }

        public async Task<List<Question>> GetByContestIdAsync(Guid contestId)
        {
            return await _context.Questions
                .AsNoTracking()
                .Include(q => q.Options)
                .Where(q => q.ContestId == contestId)
                .ToListAsync();
        }
    }
}