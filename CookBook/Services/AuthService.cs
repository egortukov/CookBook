using CookBook.Database;
using CookBook.DTOs;
using CookBook.Exceptions;
using CookBook.Models;

namespace CookBook.Services;

public class AuthService(CookBookDbContext dbContext, IJwtTokenGenerator jwtTokenGenerator) : IAuthService
{
    public User Register(RegisterDto dto)
    {
        var login = dto.Login.Trim();
        
        if (dbContext.Users.Any(u => u.Login == login))
        {
            throw new AlreadyExistsException("Пользователь с таким Логином уже существует");
        }
                
        var user = new User
        {
            Login = login,
            PasswordHash =  BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        dbContext.Users.Add(user);
        dbContext.SaveChanges();
        
        return user;
    }

    public string Login(LoginDto dto)
    {
        var login = dto.Login.Trim();
        
        var user = dbContext.Users.FirstOrDefault(u => u.Login == login);
        
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