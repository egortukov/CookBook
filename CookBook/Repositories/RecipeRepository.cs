using CookBook.Database;
using CookBook.DTOs;
using CookBook.Enums;
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

    public async Task<Recipe> AddRecipe(Recipe recipe)
    {
        await EnsureIngredientsExist(recipe.Ingredients);

        _context.Recipes.Add(recipe);

        await _context.SaveChangesAsync();

        return await GetRecipeWithIngredients(recipe.Id);
    }

    public async Task<Recipe> GetRecipe(int id)
    {
        var recipe = await _context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (recipe is null)
            throw new NotFoundException("Рецепт не найден");

        return recipe;
    }

    public async Task<List<Recipe>> GetRecipes(RecipeParametersDto parameters)
    {
        IQueryable<Recipe> query = _context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient);

        if (parameters.Name is not null)
        {
            query = query.Where(r => r.Name.Contains(parameters.Name));
        }

        if (parameters.AuthorId is not null)
        {
            query = query.Where(r => r.AuthorId == parameters.AuthorId);
        }

        if (parameters.MinRating is not null)
        {
            query = query.Where(r => r.RatingCount > 0 && r.RatingSum / r.RatingCount >= parameters.MinRating);
        }

        switch (parameters.SortBy)
        {
            case RecipeSortBy.Name:
                query = parameters.SortDescending == true
                    ? query.OrderByDescending(r => r.Name)
                    : query.OrderBy(r => r.Name);
                break;
            case RecipeSortBy.Rating:
                query = parameters.SortDescending == true
                    ? query.OrderByDescending(r => r.RatingSum / r.RatingCount)
                    : query.OrderBy(r => r.RatingSum / r.RatingCount);
                break;
        }

        return await query.ToListAsync();
    }

    public async Task<Recipe> UpdateRecipe(Recipe recipe)
    {
        await EnsureIngredientsExist(recipe.Ingredients);

        await _context.SaveChangesAsync();

        return await GetRecipeWithIngredients(recipe.Id);
    }

    public async Task DeleteRecipe(Recipe recipe)
    {
        _context.Recipes.Remove(recipe);

        await _context.SaveChangesAsync();
    }

    public async Task<Recipe> AddRating(int id, int rating)
    {
        var recipe = await GetRecipeWithIngredients(id);

        recipe.RatingSum += rating;
        recipe.RatingCount++;

        await _context.SaveChangesAsync();
        return recipe;
    }

    private async Task EnsureIngredientsExist(List<RecipeIngredient> ingredients)
    {
        foreach (var ingredient in ingredients)
            if (!await _context.Ingredients.AnyAsync(x => x.Id == ingredient.IngredientId))
                throw new NotFoundException("Ингредиент не найден");
    }

    private async Task<Recipe> GetRecipeWithIngredients(int id)
    {
        var recipe = await _context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (recipe is null)
            throw new NotFoundException("Рецепт не найден");
        return recipe;
    }
}