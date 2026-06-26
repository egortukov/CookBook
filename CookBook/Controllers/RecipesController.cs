using CookBook.DTOs;
using CookBook.Exceptions;
using CookBook.Extensions;
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
    public async Task<ActionResult<IEnumerable<RecipeDto>>> GetRecipes([FromQuery] RecipeParametersDto parameters)
    {
        var recipes = await _recipeRepository.GetRecipes(parameters);
        return recipes.Select(r => r.ToDto()).ToList();
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RecipeDto>> GetRecipe(int id)
    {
        var recipe = await _recipeRepository.GetRecipe(id);

        return Ok(recipe.ToDto());
    }

    [HttpPost]
    public async Task< ActionResult<RecipeDto>> AddRecipe(CreateRecipeDto dto)
    {
        var authorId = HttpContext.GetUserId();
        
        var recipe = await _recipeRepository.AddRecipe(dto.ToModel(authorId!.Value));

        return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RecipeDto>> UpdateRecipe(int id,UpdateRecipeDto dto)
    {
        var recipe = await EnsureUserOwnsRecipe(id);
        
        recipe.Name = dto.Name;
        recipe.Description = dto.Description;
        recipe.Ingredients.Clear();
        foreach (var i in dto.Ingredients)
            recipe.Ingredients.Add(new RecipeIngredient
            {
                IngredientId = i.IngredientId,
                Amount = i.Amount,
                Unit = i.Unit
            });

        var updated = await _recipeRepository.UpdateRecipe(recipe);
        return Ok(updated.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteRecipe(int id)
    {
        
        var recipe = await EnsureUserOwnsRecipe(id);
        
        await _recipeRepository.DeleteRecipe(recipe);

        return NoContent();
    }

    [HttpPost("{id:int}/rating")]
    public async Task<ActionResult<RecipeDto>> AddRating(int id, AddRatingDto dto)
    {
        var recipe = await _recipeRepository.AddRating(id, dto.Rating);
        return Ok(recipe.ToDto());
    }

    private async Task<Recipe> EnsureUserOwnsRecipe(int recipeId)
    {
        var userId = HttpContext.GetUserId();
        
        var recipe = await _recipeRepository.GetRecipe(recipeId);

        if (recipe.AuthorId != userId)
        {
            throw new ForbiddenException("Вы не являетесь автором этого рецепта");
        }
        
        return recipe;
    }
}