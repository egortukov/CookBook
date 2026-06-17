namespace CookBook.Models;

public class User
{
    public int Id { get; set; }
    public required string Login { get; set; }
    public required string PasswordHash { get; init; }
    public List<Recipe> Recipes { get; set; } = new();
}