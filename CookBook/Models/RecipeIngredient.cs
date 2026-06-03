namespace CookBook.Models;

public class RecipeIngredient
{
    public int Id { get; set; }
    
    public double Amount { get; set; }
    
    public Units Unit { get; set; }
    
    public int IngredientId { get; set; }
    
    public int RecipeId { get; set; }
    
    public Recipe Recipe { get; set; }
    public Ingredient Ingredient { get; set; }
    
    public enum Units
    {
        Grams, 
        Milliliters,
        Pieces,
        Tablespoons
    }
}
