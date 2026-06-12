using CookBook.DTOs;
using CookBook.Exceptions;
using CookBook.Extensions;
using CookBook.Mappings;
using CookBook.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

public class RecipesController : BaseController
{
    private const int MinRating = 1;
    private const int MaxRating = 5;

    private readonly IRecipeRepository _recipeRepository;

    public RecipesController(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public ActionResult<IEnumerable<RecipeDto>> GetRecipes()
    {
        return _recipeRepository.GetRecipes().Select(r => r.ToDto()).ToList();
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public ActionResult<RecipeDto> GetRecipe(int id)
    {
        var recipe = _recipeRepository.GetRecipe(id);

        return Ok(recipe.ToDto());
    }

    [HttpPost]
    public ActionResult<RecipeDto> AddRecipe(CreateRecipeDto dto)
    {
        var error = ValidateRecipeDto(dto.Name, dto.Description, dto.Ingredients);
        if (error != null)
        {
            return error;
        }

        var authorId = HttpContext.GetUserId();

        var recipe = _recipeRepository.AddRecipe(dto.ToModel(authorId));

        return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe.ToDto());
    }

    [HttpPut("{id:int}")]
    public ActionResult<RecipeDto> UpdateRecipe(int id, UpdateRecipeDto dto)
    {
        EnsureUserOwnsRecipe(id);
        
        var error = ValidateRecipeDto(dto.Name, dto.Description, dto.Ingredients);
        if (error != null)
        {
            return error;
        }

        var updated = _recipeRepository.UpdateRecipe(id, dto.ToModel());
        return Ok(updated.ToDto());
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteRecipe(int id)
    {
        
        EnsureUserOwnsRecipe(id);
        
        _recipeRepository.DeleteRecipe(id);

        return NoContent();
    }

    [HttpPost("{id:int}/rating")]
    public ActionResult<RecipeDto> AddRating(int id, AddRatingDto dto)
    {
        if (dto.Rating < MinRating || dto.Rating > MaxRating) return BadRequest("Оценка должна быть от 1 до 5");

        var recipe = _recipeRepository.AddRating(id, dto.Rating);
        return Ok(recipe.ToDto());
    }

    private ActionResult? ValidateRecipeDto(string name, string description, List<RecipeIngredientInputDto> ingredients)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Название рецепта обязательно");
        if (string.IsNullOrWhiteSpace(description))
            return BadRequest("Описание рецепта обязательно");
        if (ingredients == null || ingredients.Count == 0)
            return BadRequest("Рецепт должен содержать хотя бы один ингредиент");
        return null;
    }

    private void EnsureUserOwnsRecipe(int recipeId)
    {
        var userId = HttpContext.GetUserId();
        var recipe = _recipeRepository.GetRecipe(recipeId);

        if (recipe.AuthorId != userId)
        {
            throw new ForbiddenException("Вы не являетесь автором этого рецепта");
        }
    }
}