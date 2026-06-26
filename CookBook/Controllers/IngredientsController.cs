using CookBook.Database;
using CookBook.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Controllers;

public class IngredientsController : BaseController
{
    private readonly CookBookDbContext _context;

    public IngredientsController(CookBookDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<IngredientDto>>> GetIngredients()
    {
        return await _context.Ingredients
            .Select(i => new IngredientDto(i.Id, i.Name))
            .ToListAsync();
    }
}