using Microsoft.AspNetCore.Mvc;
using StockAlertApi.Core.Interfaces.Services;
using StockAlertApi.Core.Entities;

namespace StockAlertApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public AuthController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _usersService.RegisterAsync(request.Username, request.Email, request.Password);

            if (user == null)
                return BadRequest(new { message = "Username or email already exists." });

            return Ok(new { message = "User registered successfully.", user.Id, user.Username });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _usersService.LoginAsync(request.Username, request.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid credentials." });

            // jwt
            return Ok(new { message = "Login successful", user.Id, user.Username });
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
}
