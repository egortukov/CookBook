using CookBook.Enums;
using CookBook.Models;

namespace CookBook.DTOs;

public record RecipeIngredientDto(
    double Amount,
    Unit Unit,
    string IngredientName
);