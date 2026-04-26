using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantOrderAPI.API.Middleware;
using RestaurantOrderAPI.API.Services;
using RestaurantOrderAPI.Application.Interfaces;
using RestaurantOrderAPI.Application.Services;
using RestaurantOrderAPI.Domain.Interfaces;
using RestaurantOrderAPI.Infrastructure;
using RestaurantOrderAPI.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// ── MVC Controllers ──────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── Swagger / OpenAPI ────────────────────────────────────────────────────────
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Restaurant Order API",
        Version = "v1",
        Description = "Sistema de gestión de pedidos para restaurante en mercado/plaza."
    });
});

// ── Database ─────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── JWT Authentication ───────────────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        };
    });

builder.Services.AddAuthorization();

// ── Dependency Injection ─────────────────────────────────────────────────────
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// ── CORS ─────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// ── Database Initialization ──────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    await DbSeeder.SeedAsync(db);
}

// ── Middleware Pipeline ──────────────────────────────────────────────────────
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant Order API v1");
    c.RoutePrefix = string.Empty;
});

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
