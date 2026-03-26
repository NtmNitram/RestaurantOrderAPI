using RestaurantOrderAPI.Domain.Entities;

namespace RestaurantOrderAPI.Domain.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Order>> GetByClientIdAsync(int clientId);
    Task<IEnumerable<Order>> GetDailySummaryAsync(DateTime date);
    Task<Order> AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(Order order);
    Task<bool> ExistsAsync(int id);
}
