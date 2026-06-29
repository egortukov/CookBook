using CookBook.DTOs;
using CookBook.Exceptions;
using CookBook.Extensions;
using CookBook.Filters;
using CookBook.Mappings;
using CookBook.Models;
using CookBook.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

public class RecipesController : BaseController
{
    private readonly IRecipeRepository _recipeRepository;

    public RecipesController(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecipeDto>>> GetRecipes([FromQuery] RecipeFilter parameters)
    {
        var recipes = await _recipeRepository.GetRecipesAsync(parameters);
        return recipes.Select(r => r.ToDto()).ToList();
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RecipeDto>> GetRecipe(int id)
    {
        var recipe = await _recipeRepository.GetRecipeAsync(id);

        return Ok(recipe.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<RecipeDto>> AddRecipe(CreateRecipeDto dto)
    {
        var authorId = HttpContext.GetUserId();
        if (authorId is null) return Unauthorized("Пользователь не авторизован");
        var recipe = await _recipeRepository.AddRecipeAsync(dto.ToModel(authorId.Value));

        return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RecipeDto>> UpdateRecipe(int id, UpdateRecipeDto dto)
    {
        var recipe = await EnsureUserOwnsRecipe(id);
        var updated = await _recipeRepository.UpdateRecipeAsync(recipe, dto);
        return Ok(updated.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteRecipe(int id)
    {
        var recipe = await EnsureUserOwnsRecipe(id);

        await _recipeRepository.DeleteRecipeAsync(recipe);

        return NoContent();
    }

    [HttpPost("{id:int}/rating")]
    public async Task<ActionResult<RecipeDto>> AddRating(int id, AddRatingDto dto)
    {
        var recipe = await _recipeRepository.AddRatingAsync(id, dto.Rating);
        return Ok(recipe.ToDto());
    }

    private async Task<Recipe> EnsureUserOwnsRecipe(int recipeId)
    {
        var userId = HttpContext.GetUserId();

        var recipe = await _recipeRepository.GetRecipeAsync(recipeId);

        if (recipe.AuthorId != userId)
        {
            throw new ForbiddenException("Вы не являетесь автором этого рецепта");
        }

        return recipe;
    }
}