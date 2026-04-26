using RestaurantOrderAPI.Application.Helpers;
using RestaurantOrderAPI.Domain.Entities;

namespace RestaurantOrderAPI.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User { Username = "mesero", PasswordHash = PasswordHelper.Hash("mesero123"), Role = "Mesero" },
                new User { Username = "dueno",  PasswordHash = PasswordHelper.Hash("dueno123"),  Role = "Dueño" }
            );
        }

        if (!context.MenuItems.Any())
        {
            context.MenuItems.AddRange(
                new MenuItem { Name = "Desayuno Completo",  Description = "Huevos al gusto, frijoles, tortillas y café",         Price = 45.00m, IsAvailable = true },
                new MenuItem { Name = "Chilaquiles",        Description = "Chilaquiles rojos o verdes con crema y queso",          Price = 55.00m, IsAvailable = true },
                new MenuItem { Name = "Molletes",           Description = "Molletes con frijoles, queso y pico de gallo",          Price = 40.00m, IsAvailable = true },
                new MenuItem { Name = "Hotcakes",           Description = "3 hotcakes con mantequilla y miel de maple",            Price = 50.00m, IsAvailable = true },
                new MenuItem { Name = "Huevos Rancheros",   Description = "Huevos rancheros con salsa roja, frijoles y tortillas", Price = 48.00m, IsAvailable = true },
                new MenuItem { Name = "Café de Olla",       Description = "Café de olla tradicional (grande)",                    Price = 15.00m, IsAvailable = true },
                new MenuItem { Name = "Jugo Natural",       Description = "Jugo de naranja, zanahoria o betabel (500ml)",         Price = 20.00m, IsAvailable = true },
                new MenuItem { Name = "Agua de Fruta",      Description = "Agua fresca de temporada (500ml)",                     Price = 15.00m, IsAvailable = true }
            );

            context.Clients.AddRange(
                new Client { Name = "Papelería El Sol",      LocalNumber = "A-12", PhoneNumber = "555-1001", IsActive = true },
                new Client { Name = "Zapatería Martínez",    LocalNumber = "B-05", PhoneNumber = "555-1002", IsActive = true },
                new Client { Name = "Carnicería Hernández",  LocalNumber = "C-08", PhoneNumber = null,       IsActive = true },
                new Client { Name = "Ferretería González",   LocalNumber = "D-03", PhoneNumber = "555-1004", IsActive = true },
                new Client { Name = "Farmacia La Salud",     LocalNumber = "A-01", PhoneNumber = "555-1005", IsActive = true }
            );
        }

        await context.SaveChangesAsync();
    }
}
