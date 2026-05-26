namespace CookBook.Models;

public class RecipeIngredient
{
    public double Amount { get; set; }
    
    public Units Unit { get; set; }
    
    public required Ingredient Ingredient { get; set; }
    
    public enum Units
    {
        Grams, 
        Milliliters,
        Pieces,
        Tablespoons
    }
}
