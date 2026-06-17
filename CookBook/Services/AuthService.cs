using CookBook.Database;
using CookBook.DTOs;
using CookBook.Exceptions;
using CookBook.Models;

namespace CookBook.Services;

public class AuthService(CookBookDbContext dbContext, IJwtTokenGenerator jwtTokenGenerator) : IAuthService
{
    public User Register(RegisterDto dto)
    {
        if (dbContext.Users.Any(u => u.Login == dto.Login))
        {
            throw new AlreadyExistsException("Пользователь с таким Логином уже существует");
        }
                
        var user = new User
        {
            Login = dto.Login,
            PasswordHash =  BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        dbContext.Users.Add(user);
        dbContext.SaveChanges();
        
        return user;
    }

    public string Login(LoginDto dto)
    {
        var user = dbContext.Users.FirstOrDefault(u => u.Login == dto.Login);
        
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