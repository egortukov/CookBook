using CookBook.Enums;

namespace CookBook.DTOs;

public record RecipeParametersDto(
    string? Name,
    int? AuthorId,
    double? MinRating,
    RecipeSortBy? SortBy,
    bool? SortDescending
    );