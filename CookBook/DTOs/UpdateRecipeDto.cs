namespace CookBook.DTOs;

public record UpdateRecipeDto(
    string Name,
    string Description,
    List<RecipeIngredientInputDto> Ingredients
);