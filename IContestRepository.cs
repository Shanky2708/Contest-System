using ContestSystem.Dto;
using ContestSystem.Entity;
using ContestSystem.Response;

namespace ContestSystem.Interface
{


    public interface IContestRepository
    {
        Task<List<Contest>> GetAllAsync();
        Task<Contest?> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(Contest contest);   // ✅ return Guid
        Task<bool> UpdateAsync(Contest contest);
        Task<bool> DeleteAsync(Guid id);

        Task<Guid> CreateContestAsync(CreateContestDto dto);
    }

}
