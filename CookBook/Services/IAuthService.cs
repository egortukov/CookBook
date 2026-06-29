using CookBook.DTOs;
using CookBook.Models;

namespace CookBook.Services;

public interface IAuthService
{
    public Task<User> RegisterAsync(RegisterDto dto);
    public Task<string> LoginAsync(LoginDto dto);
}