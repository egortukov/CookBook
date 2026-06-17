using CookBook.Models;

namespace CookBook.Repositories;

public interface IRecipeRepository
{
    Recipe AddRecipe(Recipe recipe);
    Recipe GetRecipe(int id);
    List<Recipe> GetRecipes();
    Recipe UpdateRecipe(Recipe recipe);
    void DeleteRecipe(Recipe recipe);
    Recipe AddRating(int id, int rating);
}