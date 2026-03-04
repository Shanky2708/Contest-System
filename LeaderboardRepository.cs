using Microsoft.EntityFrameworkCore;
using ContestSystem.Interface;

namespace ContestSystem.Repositories
{
    public class LeaderboardRepository : ILeaderboardRepository
    {
        private readonly AppDbContext _context;

        public LeaderboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<(int Rank, string Email, int Score)>>
            GetLeaderboardAsync(Guid contestId)
        {
            var participants = await _context.ContestParticipants
                .Where(cp => cp.ContestId == contestId && cp.IsCompleted)
                .OrderByDescending(cp => cp.Score)
                .ThenBy(cp => cp.SubmittedAt)
                .Include(cp => cp.User)
                .ToListAsync();

            var leaderboard = new List<(int, string, int)>();

            int rank = 1;

            foreach (var participant in participants)
            {
                leaderboard.Add((
                    rank,
                    participant.User.Email,
                    participant.Score
                ));

                rank++;
            }

            return leaderboard;
        }
    }
}