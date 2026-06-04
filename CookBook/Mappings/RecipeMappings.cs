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
            Rating: recipe.RatingCount == 0 ? 0 : (double)recipe.RatingSum / recipe.RatingCount
        );
    }

    public static Recipe ToModel(this CreateRecipeDto dto)
    {
        return new Recipe
        {
            Name = dto.Name,
            Description = dto.Description,
            Ingredients = MapIngredients(dto.Ingredients)
        };
    }

    public static Recipe ToModel(this UpdateRecipeDto dto)
    {
        return new Recipe
        {
            Name = dto.Name,
            Description = dto.Description,
            Ingredients = MapIngredients(dto.Ingredients)
        };
    }
    
    private static List<RecipeIngredient> MapIngredients(List<RecipeIngredientInputDto> ingredients)
    {
        return ingredients
            .Select(i => new RecipeIngredient
            {
                Amount = i.Amount,
                Unit = i.Unit,
                IngredientId = i.IngredientId
            })
            .ToList();
    }
}