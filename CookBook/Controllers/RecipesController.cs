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
        var authorId = HttpContext.GetUserId();
        
        var recipe = _recipeRepository.AddRecipe(dto.ToModel(authorId!.Value));

        return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe.ToDto());
    }

    [HttpPut("{id:int}")]
    public ActionResult<RecipeDto> UpdateRecipe(int id,UpdateRecipeDto dto)
    {
        var recipe = EnsureUserOwnsRecipe(id);
        
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

        var updated = _recipeRepository.UpdateRecipe(recipe);
        return Ok(updated.ToDto());
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteRecipe(int id)
    {
        
        var recipe = EnsureUserOwnsRecipe(id);
        
        _recipeRepository.DeleteRecipe(recipe);

        return NoContent();
    }

    [HttpPost("{id:int}/rating")]
    public ActionResult<RecipeDto> AddRating(int id, AddRatingDto dto)
    {
        var recipe = _recipeRepository.AddRating(id, dto.Rating);
        return Ok(recipe.ToDto());
    }

    private Recipe EnsureUserOwnsRecipe(int recipeId)
    {
        var userId = HttpContext.GetUserId();
        
        var recipe = _recipeRepository.GetRecipe(recipeId);

        if (recipe.AuthorId != userId)
        {
            throw new ForbiddenException("Вы не являетесь автором этого рецепта");
        }
        
        return recipe;
    }
}