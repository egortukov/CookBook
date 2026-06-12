using CookBook.DTOs;
using CookBook.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

public class AuthController(IAuthService authService) : BaseController
{
    [AllowAnonymous]
    [HttpPost("register")]
    public ActionResult Register(RegisterDto dto)
    {
        var user = authService.Register(dto);
        return Ok(user.Id);
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<string> Login(LoginDto dto)
    {
        var token = authService.Login(dto);
        return Ok(token);
    }
}