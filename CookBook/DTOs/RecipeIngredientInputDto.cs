using CookBook.Enums;

namespace CookBook.DTOs;

public record RecipeIngredientInputDto(
    int IngredientId,
    double Amount,
    Unit Unit
);