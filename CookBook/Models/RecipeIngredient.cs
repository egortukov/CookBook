using CookBook.Enums;

namespace CookBook.Models;

public class RecipeIngredient
{
    public int Id { get; set; }
    
    public double Amount { get; set; }
    
    public Unit Unit { get; set; }
    
    public int IngredientId { get; set; }
    
    public int RecipeId { get; set; }

    public Recipe Recipe { get; set; } = null!;
    public Ingredient Ingredient { get; set; } = null!;
}
