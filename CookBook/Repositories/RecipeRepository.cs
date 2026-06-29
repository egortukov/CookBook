using CookBook.Database;
using CookBook.DTOs;
using CookBook.Enums;
using CookBook.Exceptions;
using CookBook.Filters;
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

    public async Task<Recipe> AddRecipeAsync(Recipe recipe)
    {
        await EnsureIngredientsExistAsync(recipe.Ingredients);

        _context.Recipes.Add(recipe);

        await _context.SaveChangesAsync();

        return await GetRecipeWithIngredientsAsync(recipe.Id);
    }

    public Task<Recipe> GetRecipeAsync(int id) => GetRecipeWithIngredientsAsync(id);

    public async Task<List<Recipe>> GetRecipesAsync(RecipeFilter parameters)
    {
        IQueryable<Recipe> query = _context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient);

        if (parameters.Name is not null)
        {
            query = query.Where(r => EF.Functions.ILike(r.Name, $"%{parameters.Name.Trim()}%"));
        }

        if (parameters.AuthorId is not null)
        {
            query = query.Where(r => r.AuthorId == parameters.AuthorId);
        }

        if (parameters.MinRating is not null)
        {
            query = query.Where(r => r.RatingCount > 0 && (double)r.RatingSum / r.RatingCount >= parameters.MinRating);
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
                    ? query.OrderByDescending(r => r.RatingCount == 0 ? 0 : (double)r.RatingSum / r.RatingCount)
                    : query.OrderBy(r => r.RatingCount == 0 ? 0 : (double)r.RatingSum / r.RatingCount);
                break;
        }

        return await query.ToListAsync();
    }

    public async Task<Recipe> UpdateRecipeAsync(Recipe recipe, UpdateRecipeDto dto)
    {
        recipe.Name = dto.Name.Trim();
        recipe.Description = dto.Description.Trim();
        recipe.Ingredients.Clear();
        foreach (var i in dto.Ingredients)
        {
            recipe.Ingredients.Add(new RecipeIngredient
            {
                IngredientId = i.IngredientId,
                Amount = i.Amount,
                Unit = i.Unit
            });
        }

        await EnsureIngredientsExistAsync(recipe.Ingredients);
        await _context.SaveChangesAsync();

        return await GetRecipeWithIngredientsAsync(recipe.Id);
    }

    public async Task DeleteRecipeAsync(Recipe recipe)
    {
        _context.Recipes.Remove(recipe);

        await _context.SaveChangesAsync();
    }

    public async Task<Recipe> AddRatingAsync(int id, int rating)
    {
        var recipe = await GetRecipeWithIngredientsAsync(id);

        recipe.RatingSum += rating;
        recipe.RatingCount++;

        await _context.SaveChangesAsync();
        return recipe;
    }

    private async Task EnsureIngredientsExistAsync(List<RecipeIngredient> ingredients)
    {
        var ids = ingredients.Select(i => i.IngredientId).ToList();
        var existingIds = await _context.Ingredients
            .Where(i => ids.Contains(i.Id))
            .Select(i => i.Id)
            .ToListAsync();

        var missingId = ids.FirstOrDefault(id => !existingIds.Contains(id));
        if (missingId != default)
        {
            throw new NotFoundException("Ингредиент не найден");
        }
    }

    private async Task<Recipe> GetRecipeWithIngredientsAsync(int id)
    {
        var recipe = await _context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (recipe is null)
        {
            throw new NotFoundException("Рецепт не найден");
        }
        return recipe;
    }
}