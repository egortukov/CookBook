using CookBook.Models;

namespace CookBook.Repositories;

public interface IRecipeRepository
{
    Recipe AddRecipe(Recipe recipe);
    Recipe GetRecipe(int id);
    List<Recipe> GetRecipes();
    Recipe UpdateRecipe(int id, Recipe recipe);
    void DeleteRecipe(int id);
    Recipe AddRating(int id, int rating);
}