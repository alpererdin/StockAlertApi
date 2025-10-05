using Microsoft.AspNetCore.Mvc;
using StockAlertApi.Core.Interfaces.Services;
using StockAlertApi.Infrastructure.Security;

namespace StockAlertApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsersService _usersService;
    private readonly JwtService _jwtService;

    public AuthController(IUsersService usersService, JwtService jwtService)
    {
        _usersService = usersService;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = await _usersService.RegisterAsync(request.Username, request.Email, request.Password);

        if (user == null)
            return BadRequest(new { message = "Username or email already exists." });

        var token = _jwtService.GenerateToken(user.Id, user.Username);

        return Ok(new
        {
            message = "User registered successfully.",
            token,
            userId = user.Id,
            username = user.Username
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _usersService.LoginAsync(request.Username, request.Password);

        if (user == null)
            return Unauthorized(new { message = "Invalid credentials." });

        var token = _jwtService.GenerateToken(user.Id, user.Username);

        return Ok(new
        {
            message = "Login successful",
            token,
            userId = user.Id,
            username = user.Username
        });
    }
}

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}