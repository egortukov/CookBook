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
            Password =  BCrypt.Net.BCrypt.HashPassword(dto.Password)
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
            throw new NotFoundException("Пользователь не найден");
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            throw new NotFoundException("Неверный пароль");
        }

        return jwtTokenGenerator.Generate(user);
    }
}