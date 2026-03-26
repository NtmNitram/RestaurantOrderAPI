namespace RestaurantOrderAPI.Application.DTOs;

public record CreateClientDto(
    string Nombre,
    string NumeroLocal,
    string? Telefono
);

public record UpdateClientDto(
    string Nombre,
    string NumeroLocal,
    string? Telefono,
    bool Activo
);

public record ClientResponseDto(
    int Id,
    string Nombre,
    string NumeroLocal,
    string? Telefono,
    bool Activo
);
