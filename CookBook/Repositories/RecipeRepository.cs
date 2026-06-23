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

    public Recipe AddRecipe(Recipe recipe)
    {
        EnsureIngredientsExist(recipe.Ingredients);

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

    public List<Recipe> GetRecipes(RecipeParametersDto parameters)
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

        return query.ToList();
    }

    public Recipe UpdateRecipe(Recipe recipe)
    {
        EnsureIngredientsExist(recipe.Ingredients);

        _context.SaveChanges();

        return GetRecipeWithIngredients(recipe.Id);
    }

    public void DeleteRecipe(Recipe recipe)
    {
        _context.Recipes.Remove(recipe);

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