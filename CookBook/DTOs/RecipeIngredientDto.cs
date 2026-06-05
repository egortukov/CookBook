using CookBook.Enums;

namespace CookBook.DTOs;

public record RecipeIngredientDto(
    double Amount,
    Unit Unit,
    string IngredientName
);