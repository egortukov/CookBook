using CookBook.Controllers;
using CookBook.Database;
using CookBook.Exceptions;
using CookBook.Models;
using Microsoft.EntityFrameworkCore;

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

public class RecipeRepository : IRecipeRepository
{
    private readonly CookBookDbContext _context;

    public RecipeRepository(CookBookDbContext context)
    {
        _context = context;
    }


    public Recipe AddRecipe(Recipe recipe)
    {
        foreach (var ingredient in recipe.Ingredients)
        {
            if (!_context.Ingredients.Any(x => x.Id == ingredient.IngredientId))
            {
                throw new NotFoundException("ингредиент не найден");
            }
        }

        recipe.Name = recipe.Name.Trim();
        recipe.Description = recipe.Description.Trim();

        _context.Recipes.Add(recipe);
        _context.SaveChanges();

        return _context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .First(r => r.Id == recipe.Id);
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
        var recipeToUpdate = _context.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefault(x => x.Id == id);
        
        if (recipeToUpdate == null)
        {
            throw new NotFoundException("рецепт не найден");
        }

        foreach (var ingredient in recipe.Ingredients)
        {
            if (!_context.Ingredients.Any(x => x.Id == ingredient.IngredientId))
            {
                throw new NotFoundException("ингредиент не найден");
            }
        }

        recipeToUpdate.Name = recipe.Name.Trim();
        recipeToUpdate.Description = recipe.Description.Trim();
        recipeToUpdate.Ingredients.Clear();
        foreach (var ingredient in recipe.Ingredients)
        {
            recipeToUpdate.Ingredients.Add(ingredient);
        }

        _context.SaveChanges();
        
        return _context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .First(r => r.Id == id);
    }

    public void DeleteRecipe(int id)
    {
        var recipeToDelete = _context.Recipes.FirstOrDefault(x => x.Id == id);
        if (recipeToDelete is null)
        {
            throw new NotFoundException("рецепт не найден");
        }

        _context.Recipes.Remove(recipeToDelete);

        _context.SaveChanges();
    }

    public Recipe AddRating(int id, int rating)
    {
        var recipeToRait = _context.Recipes.FirstOrDefault(x => x.Id == id);
        if (recipeToRait is null)
        {
            throw new NotFoundException("рецепт не найден");
        }

        recipeToRait.RatingSum += rating;
        recipeToRait.RatingCount++;

        _context.SaveChanges();
        
        return _context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .First(r => r.Id == id);
    }
}