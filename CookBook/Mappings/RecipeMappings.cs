using CookBook.Controllers;
using CookBook.DTOs;
using CookBook.Models;

namespace CookBook.Mappings;

public static class RecipeMappings
{
    public static RecipeDto ToDto(this Recipe recipe)
    {
        return new RecipeDto(
            Id: recipe.Id,
            Name: recipe.Name,
            Description: recipe.Description,
            Ingredients: recipe.Ingredients
                .Select(i => new RecipeIngredientDto(
                    Amount: i.Amount,
                    Unit: i.Unit,
                    IngredientName: i.Ingredient.Name
                ))
                .ToList(),
            Rating: recipe.RatingCount == 0 ? 0 : recipe.RatingSum / recipe.RatingCount
        );
    }

    public static Recipe ToModel(this CreateRecipeDto dto)
    {
        return new Recipe
        {
            Name = dto.Name,
            Description = dto.Description,
            Ingredients = dto.Ingredients
                .Select(i => new RecipeIngredient
                {
                    Amount = i.Amount,
                    Unit = i.Unit,
                    Ingredient = IngredientController.IngredientList.SingleOrDefault(ing => ing.Id == i.IngredientId)!
                })
                .ToList(),
        };
    }

    public static Recipe ToModel(this UpdateRecipeDto dto)
    {
        return new Recipe
        {
            Name = dto.Name,
            Description = dto.Description,
            Ingredients = dto.Ingredients
                .Select(i => new RecipeIngredient
                {
                    Amount = i.Amount,
                    Unit = i.Unit,
                    Ingredient = IngredientController.IngredientList.SingleOrDefault(ing => ing.Id == i.IngredientId)!
                })
                .ToList(),
        };
    }
}