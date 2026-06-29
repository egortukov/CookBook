using System.Text.Json.Serialization;
using CookBook.Configurations;
using CookBook.Database;
using CookBook.Middleware;
using CookBook.Repositories;
using CookBook.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddOptions<DatabaseOptions>()
    .Bind(builder.Configuration.GetRequiredSection("Database"))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetRequiredSection("Jwt"))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddDbContext<CookBookDbContext>((sp, options) =>
{
    var dbOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
    options.UseNpgsql(dbOptions.ConnectionString);
});
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        JwtBearerDefaults.AuthenticationScheme,
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme,
                    },
                },
                Array.Empty<string>()
            },
        });
});
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtOptions = builder.Configuration
            .GetRequiredSection("Jwt")
            .Get<JwtOptions>()!;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Convert.FromBase64String(jwtOptions.Secret))
        };
    });

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();