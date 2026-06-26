using CookBook.DTOs;
using CookBook.Models;

namespace CookBook.Services;

public interface IAuthService
{
    public Task<User> Register(RegisterDto dto);
    public Task<string> Login(LoginDto dto);
}