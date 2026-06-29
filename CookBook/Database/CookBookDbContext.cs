using CookBook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CookBook.Database;

public class CookBookDbContext : DbContext
{
    private readonly DatabaseOptions _dbOptions;
    
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<User> Users { get; set; }
    
    public CookBookDbContext(DbContextOptions<CookBookDbContext> options, IOptions<DatabaseOptions> dbOptions) : base(options)
    {
        _dbOptions = dbOptions.Value;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CookBookDbContext).Assembly);
    }
}