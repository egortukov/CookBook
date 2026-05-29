namespace CookBook.DTOs;

public record RecipeDto(
    int Id,
    string Name,
    string Description,
    List<RecipeIngredientDto> Ingredients,
    double Rating
);