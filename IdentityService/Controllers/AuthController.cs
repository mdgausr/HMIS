using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _repo;
    private readonly IConfiguration _config;
    private readonly IPasswordHasher<string> _hasher;

    public AuthController(IUserRepository repo, IConfiguration config, IPasswordHasher<string> hasher)
    {
        _repo = repo;
        _config = config;
        _hasher = hasher;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var exists = await _repo.GetByUsernameAsync(dto.Username);
        if (exists != null) return BadRequest("User exists");
        var hashed = _hasher.HashPassword(dto.Username, dto.Password);
        var user = new User { Username = dto.Username, PasswordHash = hashed, Role = dto.Role ?? "User" };
        await _repo.CreateAsync(user);
        return Ok();
    }

    [HttpPost("token")]
    public async Task<IActionResult> Token(LoginDto dto)
    {
        var user = await _repo.GetByUsernameAsync(dto.Username);
        if (user == null) return Unauthorized();
        var res = _hasher.VerifyHashedPassword(dto.Username, user.PasswordHash, dto.Password);
        if (res == PasswordVerificationResult.Failed) return Unauthorized();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );
        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}

public record RegisterDto(string Username, string Password, string? Role);
public record LoginDto(string Username, string Password);
