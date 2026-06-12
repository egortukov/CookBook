using CookBook.Models;

namespace CookBook.Services;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}