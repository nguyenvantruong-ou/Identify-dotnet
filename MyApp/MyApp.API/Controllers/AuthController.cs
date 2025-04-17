using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Models.Auth;
using MyApp.Domain.Entities;
using MyApp.API.Helpers;

namespace MyApp.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Username);
        if (user == null)
            return Unauthorized("User not found");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            return Unauthorized("Invalid credentials");

        var roles = (await _userManager.GetRolesAsync(user)).ToList();

        var jwtKey = _configuration["Jwt:Key"];
        var jwtIssuer = _configuration["Jwt:Issuer"];
        var Audience = _configuration["Jwt:Audience"];

        var token = JwtHelper.GenerateToken(user, jwtKey, jwtIssuer, Audience, roles);

        return Ok(new
        {
            Token = token,
            User = new
            {
                user.Id,
                user.Email,
                user.UserName
            }
        });
    }
}
