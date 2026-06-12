using CookBook.DTOs;
using CookBook.Models;

namespace CookBook.Mappings;

public static class RecipeMappings
{
    public static RecipeDto ToDto(this Recipe recipe)
    {
        return new RecipeDto(
            recipe.Id,
            recipe.Name,
            recipe.Description,
            recipe.Ingredients
                .Select(i => new RecipeIngredientDto(
                    i.Amount,
                    i.Unit,
                    i.Ingredient.Name
                ))
                .ToList(),
            recipe.RatingCount == 0 ? 0 : (double)recipe.RatingSum / recipe.RatingCount
        );
    }

    public static Recipe ToModel(this CreateRecipeDto dto, int authorId)
    {
        return new Recipe
        {
            AuthorId = authorId,
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