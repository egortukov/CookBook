using CookBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientController : ControllerBase
{
    public static readonly List<Ingredient> IngredientList = new()
    {
        new Ingredient { Id = 1, Name = "Мука" },
        new Ingredient { Id = 2, Name = "Соль" },
        new Ingredient { Id = 3, Name = "Сахар" },
        new Ingredient { Id = 4, Name = "Яйцо" },
        new Ingredient { Id = 5, Name = "Молоко" },
        new Ingredient { Id = 6, Name = "Масло" },
        new Ingredient { Id = 7, Name = "Вода" },
    };

    [HttpGet]
    public ActionResult<List<Ingredient>> GetIngredients() => IngredientList;
}