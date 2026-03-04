using ContestSystem.Entity;

namespace ContestSystem.Interface
{
    public interface IUserHistoryRepository
    {
        Task<List<ContestParticipant>> GetUserHistoryAsync(Guid userId);
    }
}