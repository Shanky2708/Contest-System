using Microsoft.EntityFrameworkCore;
using ContestSystem.Entity;
using ContestSystem.Interface;

namespace ContestSystem.Repositories
{
    public class UserHistoryRepository : IUserHistoryRepository
    {
        private readonly AppDbContext _context;

        public UserHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContestParticipant>> GetUserHistoryAsync(Guid userId)
        {
            return await _context.ContestParticipants
                .Include(cp => cp.Contest)
                .Where(cp => cp.UserId == userId)
                .OrderByDescending(cp => cp.SubmittedAt)
                .ToListAsync();
        }
    }
}