namespace RestaurantOrderAPI.Application.DTOs;

public record CreateClientDto(
    string Nombre,
    string Tipo,
    string? NumeroLocal,
    string? Referencia,
    string? Telefono
);

public record UpdateClientDto(
    string Nombre,
    string Tipo,
    string? NumeroLocal,
    string? Referencia,
    string? Telefono,
    bool Activo
);

public record ClientResponseDto(
    int Id,
    string Nombre,
    string Tipo,
    string? NumeroLocal,
    string? Referencia,
    string? Telefono,
    bool Activo
);
