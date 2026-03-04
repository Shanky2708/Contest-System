using ContestSystem.Entity;

namespace ContestSystem.Interface
{
    public interface IQuestionRepository
    {
        Task<Guid> AddQuestionAsync(Question question);
        Task<List<Question>> GetByContestIdAsync(Guid contestId);
    }
}