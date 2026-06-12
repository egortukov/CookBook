using CookBook.DTOs;
using CookBook.Models;

namespace CookBook.Services;

public interface IAuthService
{
    public User Register(RegisterDto dto);
    public string Login(LoginDto dto);
}