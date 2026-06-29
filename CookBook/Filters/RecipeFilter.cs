using CookBook.Enums;

namespace CookBook.Filters;

public class RecipeFilter
{
    public string? Name { get; init; }
    public int? AuthorId { get; init; }
    public double? MinRating { get; init; }
    public RecipeSortBy? SortBy { get; init; }
    public bool? SortDescending { get; init; }
}