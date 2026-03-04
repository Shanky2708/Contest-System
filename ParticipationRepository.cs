using Microsoft.EntityFrameworkCore;
using ContestSystem.Entity;
using ContestSystem.Interface;

namespace ContestSystem.Repositories
{
    public class ParticipationRepository : IParticipationRepository
    {
        private readonly AppDbContext _context;

        public ParticipationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasParticipatedAsync(Guid contestId, Guid userId)
        {
            return await _context.ContestParticipants
                .AnyAsync(cp => cp.ContestId == contestId && cp.UserId == userId);
        }

        public async Task<int> SubmitContestAsync(
            Guid contestId,
            Guid userId,
            List<(Guid QuestionId, List<Guid> SelectedOptions)> answers)
        {
            var questions = await _context.Questions
                .Include(q => q.Options)
                .Where(q => q.ContestId == contestId)
                .ToListAsync();

            int score = 0;

            foreach (var answer in answers)
            {
                var question = questions.FirstOrDefault(q => q.Id == answer.QuestionId);
                if (question == null) continue;

                var correctOptionIds = question.Options
                    .Where(o => o.IsCorrect)
                    .Select(o => o.Id)
                    .OrderBy(x => x)
                    .ToList();

                var selectedOptionIds = answer.SelectedOptions
                    .OrderBy(x => x)
                    .ToList();

                // Exact match scoring
                if (correctOptionIds.SequenceEqual(selectedOptionIds))
                {
                    score += 1;
                }
            }

            var participant = new ContestParticipant
            {
                Id = Guid.NewGuid(),
                ContestId = contestId,
                UserId = userId,
                Score = score,
                IsCompleted = true,
                SubmittedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            await _context.ContestParticipants.AddAsync(participant);
            await _context.SaveChangesAsync();

            return score;
        }
    }
}