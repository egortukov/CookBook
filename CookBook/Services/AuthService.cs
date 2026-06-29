using CookBook.Database;
using CookBook.DTOs;
using CookBook.Exceptions;
using CookBook.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Services;

public class AuthService(CookBookDbContext dbContext, IJwtTokenGenerator jwtTokenGenerator) : IAuthService
{
    public async Task<User> RegisterAsync(RegisterDto dto)
    {
        var login = dto.Login.Trim();

        if (await dbContext.Users.AnyAsync(u => u.Login == login))
        {
            throw new AlreadyExistsException("Пользователь с таким Логином уже существует");
        }

        var user = new User
        {
            Login = login,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var login = dto.Login.Trim();

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);

        if (user is null)
        {
            throw new UnauthorizedException("Неверный логин или пароль");
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Неверный логин или пароль");
        }

        return jwtTokenGenerator.Generate(user);
    }
}