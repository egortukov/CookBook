using CookBook.Database;
using CookBook.Exceptions;
using CookBook.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly CookBookDbContext _context;

    public RecipeRepository(CookBookDbContext context)
    {
        _context = context;
    }

    public Recipe AddRecipe(Recipe recipe)
    {
        EnsureIngredientsExist(recipe.Ingredients);

        recipe.Name = recipe.Name.Trim();
        recipe.Description = recipe.Description.Trim();

        _context.Recipes.Add(recipe);
        _context.SaveChanges();

        return GetRecipeWithIngredients(recipe.Id);
    }

    public Recipe GetRecipe(int id)
    {
        var recipe = _context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefault(x => x.Id == id);
        if (recipe is null)
            throw new NotFoundException("Рецепт не найден");
        return recipe;
    }

    public List<Recipe> GetRecipes()
    {
        return _context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .ToList();
    }

    public Recipe UpdateRecipe(int id, Recipe recipe)
    {
        var recipeToUpdate = GetRecipeWithIngredients(id);

        EnsureIngredientsExist(recipe.Ingredients);

        recipeToUpdate.Name = recipe.Name.Trim();
        recipeToUpdate.Description = recipe.Description.Trim();
        recipeToUpdate.Ingredients.Clear();
        foreach (var ingredient in recipe.Ingredients) recipeToUpdate.Ingredients.Add(ingredient);

        _context.SaveChanges();

        return recipeToUpdate;
    }

    public void DeleteRecipe(int id)
    {
        var recipeToDelete = GetRecipeWithIngredients(id);

        _context.Recipes.Remove(recipeToDelete);

        _context.SaveChanges();
    }

    public Recipe AddRating(int id, int rating)
    {
        var recipe = GetRecipeWithIngredients(id);

        recipe.RatingSum += rating;
        recipe.RatingCount++;

        _context.SaveChanges();
        return recipe;
    }

    private void EnsureIngredientsExist(List<RecipeIngredient> ingredients)
    {
        foreach (var ingredient in ingredients)
            if (!_context.Ingredients.Any(x => x.Id == ingredient.IngredientId))
                throw new NotFoundException("Ингредиент не найден");
    }

    private Recipe GetRecipeWithIngredients(int id)
    {
        var recipe = _context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefault(r => r.Id == id);
        if (recipe is null)
            throw new NotFoundException("Рецепт не найден");
        return recipe;
    }
}