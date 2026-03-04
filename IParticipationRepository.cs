using ContestSystem.Entity;

namespace ContestSystem.Interface
{
    public interface IParticipationRepository
    {
        Task<bool> HasParticipatedAsync(Guid contestId, Guid userId);
        Task<int> SubmitContestAsync(Guid contestId, Guid userId, List<(Guid QuestionId, List<Guid> SelectedOptions)> answers);
    }
}