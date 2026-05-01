using RestaurantOrderAPI.Domain.Enums;

namespace RestaurantOrderAPI.Application.DTOs;

public record CreateOrderDetailDto(
    int ArticuloId,
    int Cantidad
);

public record CreateOrderDto(
    int ClienteId,
    string? Notas,
    List<CreateOrderDetailDto> Articulos
);

public record ChangeOrderStatusDto(
    OrderStatus Estado
);

public record OrderDetailResponseDto(
    int Id,
    int ArticuloId,
    string NombreArticulo,
    int Cantidad,
    decimal PrecioUnitario,
    decimal Subtotal
);

public record OrderResponseDto(
    int Id,
    int ClienteId,
    string NombreCliente,
    string? LocalCliente,
    string? ReferenciaCliente,
    DateTime FechaPedido,
    string Estado,
    string? Notas,
    decimal Total,
    List<OrderDetailResponseDto> Articulos
);
