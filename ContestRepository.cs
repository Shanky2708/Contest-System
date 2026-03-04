using ContestSystem.Dto;
using ContestSystem.Entity;
using ContestSystem.Enum;
using ContestSystem.Interface;
using Microsoft.EntityFrameworkCore;

namespace ContestSystem.Repositories
{
    public class ContestRepository : IContestRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ContestRepository> _logger;

        public ContestRepository(AppDbContext context,
                                 ILogger<ContestRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Contest>> GetAllAsync()
        {
            try
            {
                var contests = await _context.Contests
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(c => new Contest
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        AccessLevel = c.AccessLevel,
                        StartTime = c.StartTime,
                        EndTime = c.EndTime,
                        Prize = c.Prize,
                        CreatedAt = c.CreatedAt
                    })
                    .ToListAsync();

                _logger.LogInformation("Successfully retrieved {Count} contests at {Time}",
                    contests.Count, DateTime.UtcNow);

                return contests;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx,
                    "Database error occurred while retrieving contests at {Time}",
                    DateTime.UtcNow);

                throw; // Let global middleware handle it
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error occurred while retrieving contests at {Time}",
                    DateTime.UtcNow);

                throw;
            }
        }

        public async Task<Contest> GetByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    _logger.LogWarning("GetByIdAsync called with empty Guid at {Time}", DateTime.UtcNow);
                    return null;
                }

                var contest = await _context.Contests
                    .AsNoTracking()
                    .Where(c => c.Id == id)
                    .Select(c => new Contest
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        AccessLevel = c.AccessLevel,
                        StartTime = c.StartTime,
                        EndTime = c.EndTime,
                        Prize = c.Prize,
                        CreatedAt = c.CreatedAt
                    })
                    .FirstOrDefaultAsync();

                if (contest == null)
                {
                    _logger.LogInformation("Contest not found for Id: {ContestId} at {Time}",
                        id, DateTime.UtcNow);
                }
                else
                {
                    _logger.LogInformation("Contest retrieved successfully for Id: {ContestId}",
                        id);
                }

                return contest;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx,
                    "Database error while retrieving contest Id: {ContestId} at {Time}",
                    id, DateTime.UtcNow);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error while retrieving contest Id: {ContestId} at {Time}",
                    id, DateTime.UtcNow);

                throw;
            }
        }

        public async Task<Guid> AddAsync(Contest contest)
        {
            try
            {
                if (contest == null)
                {
                    _logger.LogWarning("Attempted to add null contest at {Time}", DateTime.UtcNow);
                    throw new ArgumentNullException(nameof(contest));
                }

                contest.Id = Guid.NewGuid();
                contest.CreatedAt = DateTime.UtcNow;

                await _context.Contests.AddAsync(contest);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Contest created successfully with Id: {ContestId} at {Time}",
                    contest.Id, DateTime.UtcNow);

                return contest.Id;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx,
                    "Database error occurred while adding contest at {Time}",
                    DateTime.UtcNow);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error occurred while adding contest at {Time}",
                    DateTime.UtcNow);

                throw;
            }
        }

        public async Task<bool> UpdateAsync(Contest contest)
        {
            try
            {
                if (contest == null || contest.Id == Guid.Empty)
                {
                    _logger.LogWarning("Invalid contest data provided for update at {Time}", DateTime.UtcNow);
                    return false;
                }

                var existingContest = await _context.Contests
                    .FirstOrDefaultAsync(c => c.Id == contest.Id);

                if (existingContest == null)
                {
                    _logger.LogInformation("Contest not found for update. Id: {ContestId}", contest.Id);
                    return false;
                }

                // Update fields
                existingContest.Name = contest.Name;
                existingContest.Description = contest.Description;
                existingContest.AccessLevel = contest.AccessLevel;
                existingContest.StartTime = contest.StartTime;
                existingContest.EndTime = contest.EndTime;
                existingContest.Prize = contest.Prize;

                var result = await _context.SaveChangesAsync();

                _logger.LogInformation("Contest updated successfully. Id: {ContestId}", contest.Id);

                return result > 0;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx,
                    "Database error while updating contest Id: {ContestId}",
                    contest?.Id);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error while updating contest Id: {ContestId}",
                    contest?.Id);

                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    _logger.LogWarning("Delete attempted with empty Guid at {Time}", DateTime.UtcNow);
                    return false;
                }

                var contest = await _context.Contests
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (contest == null)
                {
                    _logger.LogInformation("Contest not found for deletion. Id: {ContestId}", id);
                    return false;
                }

                _context.Contests.Remove(contest);

                var result = await _context.SaveChangesAsync();

                _logger.LogInformation("Contest deleted successfully. Id: {ContestId}", id);

                return result > 0;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx,
                    "Database error while deleting contest Id: {ContestId}",
                    id);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error while deleting contest Id: {ContestId}",
                    id);

                throw;
            }
        }

        public async Task<Guid> CreateContestAsync(CreateContestDto dto)
        {
            // 🔎 Basic Validations
            if (dto.EndTime <= dto.StartTime)
                throw new Exception("EndTime must be greater than StartTime");

            if (dto.Questions == null || !dto.Questions.Any())
                throw new Exception("Contest must contain at least one question");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var contest = new Contest
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Description = dto.Description,
                    AccessLevel = (AccessLevel)dto.AccessLevel,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    Prize = dto.Prize,
                    CreatedAt = DateTime.UtcNow,
                    Questions = new List<Question>()
                };

                foreach (var questionDto in dto.Questions)
                {
                    if (questionDto.Options == null || questionDto.Options.Count < 2)
                        throw new Exception("Each question must have at least 2 options");

                    var correctCount = questionDto.Options.Count(o => o.IsCorrect);

                    if (correctCount == 0)
                        throw new Exception("Each question must have at least one correct option");

                    if ((QuestionType)questionDto.Type == QuestionType.SingleSelect && correctCount > 1)
                        throw new Exception("Single select question cannot have multiple correct answers");

                    var question = new Question
                    {
                        Id = Guid.NewGuid(),
                        Text = questionDto.Text,
                        Type = (QuestionType)questionDto.Type,
                        CreatedAt = DateTime.UtcNow,
                        Options = new List<Option>()
                    };

                    foreach (var optionDto in questionDto.Options)
                    {
                        var option = new Option
                        {
                            Id = Guid.NewGuid(),
                            Text = optionDto.Text,
                            IsCorrect = optionDto.IsCorrect,
                            CreatedAt = DateTime.UtcNow
                        };

                        question.Options.Add(option);
                    }

                    contest.Questions.Add(question);
                }

                _context.Contests.Add(contest);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return contest.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}