using RestaurantOrderAPI.Application.DTOs;
using RestaurantOrderAPI.Application.Interfaces;
using RestaurantOrderAPI.Domain.Entities;
using RestaurantOrderAPI.Domain.Enums;
using RestaurantOrderAPI.Domain.Interfaces;

namespace RestaurantOrderAPI.Application.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();
        return orders.Select(MapToResponse);
    }

    public async Task<OrderResponseDto> GetByIdAsync(int id)
    {
        var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(id)
            ?? throw new KeyNotFoundException($"Pedido con ID {id} no encontrado.");
        return MapToResponse(order);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetByClientIdAsync(int clientId)
    {
        if (!await _unitOfWork.Clients.ExistsAsync(clientId))
            throw new KeyNotFoundException($"Cliente con ID {clientId} no encontrado.");

        var orders = await _unitOfWork.Orders.GetByClientIdAsync(clientId);
        return orders.Select(MapToResponse);
    }

    public async Task<OrderResponseDto> CreateAsync(CreateOrderDto dto)
    {
        if (!await _unitOfWork.Clients.ExistsAsync(dto.ClienteId))
            throw new KeyNotFoundException($"Cliente con ID {dto.ClienteId} no encontrado.");

        if (dto.Articulos == null || dto.Articulos.Count == 0)
            throw new InvalidOperationException("El pedido debe tener al menos un artículo.");

        var order = new Order
        {
            ClientId = dto.ClienteId,
            OrderDate = DateTime.Now,
            Status = OrderStatus.Pending,
            Notes = dto.Notas,
            OrderDetails = new List<OrderDetail>()
        };

        decimal total = 0;
        foreach (var item in dto.Articulos)
        {
            if (item.Cantidad <= 0)
                throw new ArgumentException($"La cantidad debe ser mayor a cero para el artículo {item.ArticuloId}.");

            var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(item.ArticuloId)
                ?? throw new KeyNotFoundException($"Platillo con ID {item.ArticuloId} no encontrado.");

            if (!menuItem.IsAvailable)
                throw new InvalidOperationException($"El platillo '{menuItem.Name}' no está disponible.");

            var subtotal = menuItem.Price * item.Cantidad;
            total += subtotal;

            order.OrderDetails.Add(new OrderDetail
            {
                MenuItemId = item.ArticuloId,
                Quantity = item.Cantidad,
                UnitPrice = menuItem.Price,
                Subtotal = subtotal
            });
        }

        order.Total = total;
        var created = await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        var orderWithDetails = await _unitOfWork.Orders.GetByIdWithDetailsAsync(created.Id)
            ?? throw new InvalidOperationException("Error al recuperar el pedido creado.");
        return MapToResponse(orderWithDetails);
    }

    public async Task<OrderResponseDto> ChangeStatusAsync(int id, ChangeOrderStatusDto dto)
    {
        var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(id)
            ?? throw new KeyNotFoundException($"Pedido con ID {id} no encontrado.");

        ValidarTransicionEstado(order.Status, dto.Estado);

        order.Status = dto.Estado;
        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
        return MapToResponse(order);
    }

    public async Task DeleteAsync(int id)
    {
        var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(id)
            ?? throw new KeyNotFoundException($"Pedido con ID {id} no encontrado.");
        await _unitOfWork.Orders.DeleteAsync(order);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<DailySummaryDto> GetDailySummaryAsync(DateTime date)
    {
        var orders = await _unitOfWork.Orders.GetDailySummaryAsync(date);

        var clientGroups = orders
            .GroupBy(o => o.ClientId)
            .Select(g =>
            {
                var client = g.First().Client;
                var pedidosCobrar = g
                    .Where(o => o.Status != OrderStatus.Cancelled)
                    .OrderBy(o => o.OrderDate)
                    .ToList();

                return new ClientDailySummaryDto(
                    client.Id,
                    client.Name,
                    client.LocalNumber,
                    client.PhoneNumber,
                    pedidosCobrar.Select(o => new OrderSummaryItemDto(
                        o.Id,
                        o.OrderDate,
                        TraducirEstado(o.Status),
                        o.Notes,
                        o.Total,
                        o.OrderDetails.Select(d => new OrderDetailResponseDto(
                            d.Id,
                            d.MenuItemId,
                            d.MenuItem.Name,
                            d.Quantity,
                            d.UnitPrice,
                            d.Subtotal
                        )).ToList()
                    )).ToList(),
                    pedidosCobrar.Sum(o => o.Total)
                );
            })
            .OrderBy(c => c.NumeroLocal)
            .ToList();

        return new DailySummaryDto(
            date.Date,
            orders.Count(o => o.Status != OrderStatus.Cancelled),
            clientGroups,
            clientGroups.Sum(c => c.TotalACobrar)
        );
    }

    private static void ValidarTransicionEstado(OrderStatus actual, OrderStatus nuevo)
    {
        var valido = (actual, nuevo) switch
        {
            (OrderStatus.Pending, OrderStatus.Delivered) => true,
            (OrderStatus.Pending, OrderStatus.Cancelled) => true,
            (OrderStatus.Delivered, OrderStatus.Cancelled) => true,
            _ => false
        };

        if (!valido)
            throw new InvalidOperationException(
                $"No se puede cambiar el estado del pedido de '{TraducirEstado(actual)}' a '{TraducirEstado(nuevo)}'.");
    }

    private static string TraducirEstado(OrderStatus estado) => estado switch
    {
        OrderStatus.Pending => "Pendiente",
        OrderStatus.Delivered => "Entregado",
        OrderStatus.Cancelled => "Cancelado",
        _ => estado.ToString()
    };

    private static OrderResponseDto MapToResponse(Order o) =>
        new(
            o.Id,
            o.ClientId,
            o.Client?.Name ?? string.Empty,
            o.Client?.LocalNumber ?? string.Empty,
            o.OrderDate,
            TraducirEstado(o.Status),
            o.Notes,
            o.Total,
            o.OrderDetails.Select(d => new OrderDetailResponseDto(
                d.Id,
                d.MenuItemId,
                d.MenuItem?.Name ?? string.Empty,
                d.Quantity,
                d.UnitPrice,
                d.Subtotal
            )).ToList()
        );
}
