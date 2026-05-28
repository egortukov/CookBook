using CookBook.Models;
using CookBook.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly IRecipeRepository _recipeRepository;

    public RecipesController(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Recipe>> GetRecipes() => _recipeRepository.GetRecipes();

    [HttpGet("{id:int}")]
    public ActionResult<Recipe> GetRecipe(int id)
    {
        var recipe = _recipeRepository.GetRecipe(id);

        return Ok(recipe);
    }

    [HttpPost]
    public ActionResult<Recipe> AddRecipe(Recipe recipe)
    {
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

        _recipeRepository.AddRecipe(recipe);
        return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Recipe> UpdateRecipe(int id, Recipe recipe)
    {
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

        var updated = _recipeRepository.UpdateRecipe(id, recipe);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteRecipe(int id)
    {
        _recipeRepository.DeleteRecipe(id);

        return NoContent();
    }

    [HttpPost("{id}/rating")]
    public ActionResult<Recipe> AddRaiting(int id, [FromBody] int rating)
    {
        if (rating < 1 || rating > 5)
            return BadRequest();

        var recipe = _recipeRepository.AddRating(id, rating);
        return Ok(recipe);
    }
}