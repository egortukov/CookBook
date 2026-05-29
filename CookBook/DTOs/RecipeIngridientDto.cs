using CookBook.Models;

namespace CookBook.DTOs;

public record RecipeIngredientDto(
    double Amount,
    RecipeIngredient.Units Unit,
    string IngredientName
);