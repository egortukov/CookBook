using CookBook.DTOs;
using CookBook.Models;

namespace CookBook.Repositories;

public interface IRecipeRepository
{
    Task<Recipe> AddRecipe(Recipe recipe);
    Task<Recipe> GetRecipe(int id);
    Task<List<Recipe>> GetRecipes(RecipeParametersDto parameters);
    Task<Recipe> UpdateRecipe(Recipe recipe);
    Task DeleteRecipe(Recipe recipe);
    Task<Recipe> AddRating(int id, int rating);
}