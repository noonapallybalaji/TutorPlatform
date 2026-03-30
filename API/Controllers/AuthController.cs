namespace API.Controllers;

using API.Services;
using Application.AUTH.DTOs;
using Infrastructure.Auth;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtService _jwtService;
    private readonly AuthService _authService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        JwtService jwtService,
        AuthService authService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        return Ok(new { message = result });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return Unauthorized("Invalid credentials");

        var valid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!valid)
            return Unauthorized("Invalid credentials");

        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtService.GenerateToken(user, roles);

        return Ok(new
        {
            token,
            user.Email,
            roles
        });
    }

    [HttpGet("check-email")]
    public async Task<IActionResult> CheckEmail([FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { message = "Email is required" });

        email = email.Trim().ToLower();

        var user = await _userManager.FindByEmailAsync(email);

        var exists = user != null;

        return Ok(new
        {
            exists // 👈 THIS is what your UI expects
        });
    }
}