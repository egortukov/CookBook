using CookBook.Database;
using CookBook.Middleware;
using CookBook.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOptions<DatabaseOptions>()
    .Bind(builder.Configuration.GetRequiredSection("Database"))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddDbContext<CookBookDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();