using Microsoft.EntityFrameworkCore;
using RestaurantOrderAPI.API.Middleware;
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
        Description = "Sistema de gestión de pedidos para restaurante en mercado/plaza. " +
                      "Permite registrar clientes, menú, pedidos y generar resumen de cobro diario."
    });
});

// ── Database ─────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Dependency Injection ─────────────────────────────────────────────────────
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// ── CORS (allow any origin for local network access) ─────────────────────────
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
    c.RoutePrefix = string.Empty; // Swagger UI at root: http://localhost:5000
});

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
