using CookBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private static readonly List<Recipe> Recipes = new();
    
    [HttpGet]
    public ActionResult<IEnumerable<Recipe>> GetRecipes() => Recipes;

    [HttpGet("{id}")]
    public ActionResult<Recipe> GetRecipe(int id)
    {
        var recipe = Recipes.SingleOrDefault(x => x.Id == id);

        if (recipe == null)
        {
            return NotFound();
        }

        return Ok(recipe);
    }

    [HttpPost]
    public ActionResult<Recipe> AddRecipe(Recipe recipe)
    {
       Recipes.Add(recipe);
        return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe);
    }

    [HttpPut("{id}")]
    public ActionResult<Recipe> UpdateRecipe(int id, Recipe recipe)
    {
        var recipeToUpdate = Recipes.SingleOrDefault(x => x.Id == id);
        if (recipeToUpdate == null)
        {
            return NotFound();
        }
            recipeToUpdate.Name = recipe.Name;
            recipeToUpdate.Description = recipe.Description;

        return Ok(recipeToUpdate);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteRecipe(int id)
    {
        var recipeToDelete = Recipes.SingleOrDefault(x => x.Id == id);
        if (recipeToDelete == null)
        {
            return NotFound();
        }
        
        Recipes.Remove(recipeToDelete);
        
        return NoContent();
    }
}