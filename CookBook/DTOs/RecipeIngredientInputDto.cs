using CookBook.Models;

namespace CookBook.DTOs;

public record RecipeIngredientInputDto(
    int IngredientId,
    double Amount,
    RecipeIngredient.Units Unit
);