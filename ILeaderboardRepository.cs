using ContestSystem.Entity;

namespace ContestSystem.Interface
{
    public interface ILeaderboardRepository
    {
        Task<List<(int Rank, string Email, int Score)>>
            GetLeaderboardAsync(Guid contestId);
    }
}