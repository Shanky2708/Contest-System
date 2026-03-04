using ContestSystem.Dto;
using ContestSystem.Response;

namespace ContestSystem.Interface
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}
