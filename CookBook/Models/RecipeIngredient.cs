namespace CookBook.Models;

public class RecipeIngredient
{
    public int Id { get; set; }
    public int IngredientId { get; set; }
    public double Amount { get; set; }
    public string Unit { get; set; }
    public Ingredient Ingredient { get; set; }
}