using CookBook.DTOs;
using CookBook.Mappings;
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
    public ActionResult<IEnumerable<RecipeDto>> GetRecipes() => _recipeRepository.GetRecipes().Select(r => r.ToDto()).ToList();

    [HttpGet("{id:int}")]
    public ActionResult<RecipeDto> GetRecipe(int id)
    {
        var recipe = _recipeRepository.GetRecipe(id);

        return Ok(recipe.ToDto());
    }

    [HttpPost]
    public ActionResult<RecipeDto> AddRecipe(CreateRecipeDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(dto.Description))
        {
            return BadRequest();
        }

        if (dto.Ingredients.Count <= 0)
        {
            return BadRequest();
        }
        
        var recipe = _recipeRepository.AddRecipe(dto.ToModel());
        
        return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe.ToDto());
    }

    [HttpPut("{id:int}")]
    public ActionResult<RecipeDto> UpdateRecipe(int id, UpdateRecipeDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(dto.Description))
        {
            return BadRequest();
        }

        if (dto.Ingredients.Count <= 0)
        {
            return BadRequest();
        }

        var updated = _recipeRepository.UpdateRecipe(id, dto.ToModel());
        return Ok(updated.ToDto());
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteRecipe(int id)
    {
        _recipeRepository.DeleteRecipe(id);

        return NoContent();
    }

    [HttpPost("{id}/rating")]
    public ActionResult<RecipeDto> AddRaiting(int id, AddRatingDto dto)
    {
        if ( dto.Rating < 1 ||  dto.Rating > 5)
            return BadRequest();

        var recipe = _recipeRepository.AddRating(id,  dto.Rating);
        return Ok(recipe.ToDto());
    }
}