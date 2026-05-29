namespace CookBook.DTOs;

public record CreateRecipeDto(
    string Name,
    string Description,
    List<RecipeIngredientInputDto> Ingredients
);