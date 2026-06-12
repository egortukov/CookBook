namespace CookBook.Models;

public class Recipe
{
    public int Id { get; init; }
    public int AuthorId { get; init; }
    public User Author { get; init; } = null!;
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<RecipeIngredient> Ingredients { get; init; } = new();
    public int RatingCount { get; set; }
    public int RatingSum { get; set; }
}