using CookBook.Enums;
using CookBook.Models;

namespace CookBook.DTOs;

public record RecipeIngredientInputDto(
    int IngredientId,
    double Amount,
    Unit Unit
);