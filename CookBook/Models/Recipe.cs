namespace CookBook.Models;

public class Recipe
{
    public int Id { get; init; }
    public required string Name { get; set; } 
    public required string Description { get; set; }
    public List<RecipeIngredient> Ingredients { get; init; } = new();
    public int RatingCount { get; set; }
    public double RatingSum { get; set; }
}