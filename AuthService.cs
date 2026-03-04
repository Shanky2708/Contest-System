using ContestSystem.Dto;
using ContestSystem.Entit;
using ContestSystem.Entity;
using ContestSystem.Enum;
using ContestSystem.Interface;
using ContestSystem.Response;
using DocumentFormat.OpenXml.Math;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ContestSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
                return false;

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = (Microsoft.VisualStudio.Services.UserAccountMapping.UserRole)(ContestSystem.Enum.UserRole)dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            var token = GenerateJwtToken(user);

            return token;
        }

        private AuthResponseDto GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var expires = DateTime.UtcNow.AddMinutes(
                double.Parse(jwtSettings["ExpiryMinutes"]));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expires
            };
        }
    }
}