using construtivaBack.DTOs;
using construtivaBack.Models;
using construtivaBack.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text;

namespace construtivaBack.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterUserAsync(model);

        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code == "DuplicateEmail"))
            {
                return Conflict(new { Message = result.Errors.First().Description });
            }
            return BadRequest(new { Message = "Erro ao registrar usuário.", Errors = result.Errors });
        }

        return Ok(new { Status = "Success", Message = "Usuário criado com sucesso!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userToken = await _authService.LoginUserAsync(model);
        if (userToken == null)
        {
            return Unauthorized(new { Message = "Email ou senha inválidos." });
        }
        return Ok(userToken);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var userInfo = await _authService.GetUserInfoAsync(userId);
        if (userInfo == null)
        {
            return NotFound();
        }

        return Ok(userInfo);
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateUserDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var result = await _authService.UpdateUserAsync(userId, model);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }
}