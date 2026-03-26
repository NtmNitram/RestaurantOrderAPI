namespace RestaurantOrderAPI.Application.DTOs;

public record CreateMenuItemDto(
    string Nombre,
    string? Descripcion,
    decimal Precio
);

public record UpdateMenuItemDto(
    string Nombre,
    string? Descripcion,
    decimal Precio,
    bool Disponible
);

public record MenuItemResponseDto(
    int Id,
    string Nombre,
    string? Descripcion,
    decimal Precio,
    bool Disponible
);
