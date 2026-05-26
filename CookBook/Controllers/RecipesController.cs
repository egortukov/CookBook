using CookBook.Exceptions;
using CookBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private static readonly List<Recipe> Recipes = new();
    private Recipe? Find(int id) => Recipes.SingleOrDefault(x => x.Id == id);

    [HttpGet]
    public ActionResult<IEnumerable<Recipe>> GetRecipes() => Recipes.ToList();

    [HttpGet("{id:int}")]
    public ActionResult<Recipe> GetRecipe(int id)
    {
        var recipe = Find(id);

        if (recipe is null)
        {
            throw new NotFoundException("Рецепт не найден");
        }

        return Ok(recipe);
    }

    [HttpPost]
    public ActionResult<Recipe> AddRecipe(Recipe recipe)
    {
        if (Find(recipe.Id) is not null)
        {
            throw new AlreadyExistsException("такой рецепт уже существует");
        }
        
        if (string.IsNullOrWhiteSpace(recipe.Name))
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(recipe.Description))
        {
            return BadRequest();
        }

        if (recipe.Ingredients.Count <= 0)
        {
            return BadRequest();
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

        Recipes.Add(recipe);
        return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Recipe> UpdateRecipe(int id, Recipe recipe)
    {
        var recipeToUpdate = Find(id);
        if (recipeToUpdate == null)
        {
            throw new NotFoundException("рецепт не найден");
        }

        if (string.IsNullOrWhiteSpace(recipe.Name))
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(recipe.Description))
        {
            return BadRequest();
        }
        
        if (recipe.Ingredients.Count <= 0)
        {
            return BadRequest();
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

        return Ok(recipeToUpdate);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteRecipe(int id)
    {
        var recipeToDelete = Find(id);
        if (recipeToDelete is null)
        {
            throw new NotFoundException("рецепт не найден");
        }

        Recipes.Remove(recipeToDelete);

        return NoContent();
    }

    [HttpPost("{id}/rating")]
    public ActionResult<Recipe> AddRaiting(int id, [FromBody] int rating)
    {
        var recipeToRait = Find(id);
        if (recipeToRait is null)
        {
            throw new NotFoundException("рецепт не найден");
        }

        if (rating < 1 || rating > 5)
        {
            return BadRequest();
        }

        recipeToRait.RatingSum += rating;
        recipeToRait.RatingCount++;

        return Ok(recipeToRait);
    }
}