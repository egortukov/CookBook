using CookBook.DTOs;
using CookBook.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

public class AuthController(IAuthService authService) : BaseController
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto dto)
    {
        var user = await authService.RegisterAsync(dto);
        return Ok(user.Id);
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginDto dto)
    {
        var token = await authService.LoginAsync(dto);
        return Ok(token);
    }
}