using RestaurantOrderAPI.Application.DTOs;

namespace RestaurantOrderAPI.Application.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderResponseDto>> GetAllAsync();
    Task<OrderResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<OrderResponseDto>> GetByClientIdAsync(int clientId);
    Task<OrderResponseDto> CreateAsync(CreateOrderDto dto);
    Task<OrderResponseDto> ChangeStatusAsync(int id, ChangeOrderStatusDto dto);
    Task DeleteAsync(int id);
    Task<DailySummaryDto> GetDailySummaryAsync(DateTime date);
}
