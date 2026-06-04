using CookBook.Database;
using CookBook.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController : ControllerBase
{
    private readonly CookBookDbContext _context;

    public IngredientsController(CookBookDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<List<IngredientDto>> GetIngredients()
    {
        return _context.Ingredients
            .Select(i => new IngredientDto(i.Id, i.Name))
            .ToList();
    }
}