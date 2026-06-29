using CookBook.DTOs;
using CookBook.Filters;
using CookBook.Models;

namespace CookBook.Repositories;

public interface IRecipeRepository
{
    Task<Recipe> AddRecipeAsync(Recipe recipe);
    Task<Recipe> GetRecipeAsync(int id);
    Task<List<Recipe>> GetRecipesAsync(RecipeFilter parameters);
    Task<Recipe> UpdateRecipeAsync(Recipe recipe, UpdateRecipeDto dto);
    Task DeleteRecipeAsync(Recipe recipe);
    Task<Recipe> AddRatingAsync(int id, int rating);
}