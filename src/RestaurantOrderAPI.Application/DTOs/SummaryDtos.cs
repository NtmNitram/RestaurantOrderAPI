namespace RestaurantOrderAPI.Application.DTOs;

public record OrderSummaryItemDto(
    int PedidoId,
    DateTime FechaPedido,
    string Estado,
    string? Notas,
    decimal Total,
    List<OrderDetailResponseDto> Articulos
);

public record ClientDailySummaryDto(
    int ClienteId,
    string NombreCliente,
    string NumeroLocal,
    string? Telefono,
    List<OrderSummaryItemDto> Pedidos,
    decimal TotalACobrar
);

public record DailySummaryDto(
    DateTime Fecha,
    int TotalPedidos,
    List<ClientDailySummaryDto> Clientes,
    decimal TotalGeneral
);
