using CookBook.Controllers;
using CookBook.Exceptions;
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

public class RecipeRepository : IRecipeRepository
{
    private readonly List<Recipe> _recipes = new();

    private Recipe? Find(int id) => _recipes.SingleOrDefault(x => x.Id == id);

    public Recipe AddRecipe(Recipe recipe)
    {
        if (Find(recipe.Id) is not null)
        {
            throw new AlreadyExistsException("такой рецепт уже существует");
        }

        foreach (var ingredient in recipe.Ingredients)
        {
            if (!IngredientController.IngredientList.Any(x => x.Id == ingredient.Ingredient.Id))
            {
                throw new NotFoundException("ингредиент не найден");
            }
        }

        recipe.Name = recipe.Name.Trim();
        recipe.Description = recipe.Description.Trim();

        _recipes.Add(recipe);
        return recipe;
    }

    public Recipe GetRecipe(int id)
    {
        var recipe = Find(id);
        if (recipe is null)
            throw new NotFoundException("Рецепт не найден");
        return recipe;
    }

    public List<Recipe> GetRecipes()
    {
        return _recipes.ToList();
    }

    public Recipe UpdateRecipe(int id, Recipe recipe)
    {
        var recipeToUpdate = Find(id);
        if (recipeToUpdate == null)
        {
            throw new NotFoundException("рецепт не найден");
        }

        foreach (var ingredient in recipe.Ingredients)
        {
            if (!IngredientController.IngredientList.Any(x => x.Id == ingredient.Ingredient.Id))
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

        return recipeToUpdate;
    }

    public void DeleteRecipe(int id)
    {
        var recipeToDelete = Find(id);
        if (recipeToDelete is null)
        {
            throw new NotFoundException("рецепт не найден");
        }

        _recipes.Remove(recipeToDelete);

        return;
    }

    public Recipe AddRating(int id, int rating)
    {
        var recipeToRait = Find(id);
        if (recipeToRait is null)
        {
            throw new NotFoundException("рецепт не найден");
        }
        
        recipeToRait.RatingSum += rating;
        recipeToRait.RatingCount++;

        return recipeToRait;
    }
}