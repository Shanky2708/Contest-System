using ContestSystem.Dto;
using ContestSystem.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ContestSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService,
                              ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        // ----------------------------
        // REGISTER
        // ----------------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(dto);

            if (!result)
            {
                _logger.LogWarning("Registration failed for Email: {Email}", dto.Email);
                return BadRequest(new { message = "User already exists." });
            }

            return Ok(new { message = "User registered successfully." });
        }

        // ----------------------------
        // LOGIN
        // ----------------------------
        [EnableRateLimiting("fixed")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(dto);

            if (result == null)
            {
                _logger.LogWarning("Login failed for Email: {Email}", dto.Email);
                return Unauthorized(new { message = "Invalid credentials." });
            }

            return Ok(result);
        }
    }
}