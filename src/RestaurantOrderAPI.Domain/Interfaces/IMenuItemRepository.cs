using RestaurantOrderAPI.Domain.Entities;

namespace RestaurantOrderAPI.Domain.Interfaces;

public interface IMenuItemRepository
{
    Task<IEnumerable<MenuItem>> GetAllAsync();
    Task<IEnumerable<MenuItem>> GetAvailableAsync();
    Task<MenuItem?> GetByIdAsync(int id);
    Task<MenuItem> AddAsync(MenuItem menuItem);
    Task UpdateAsync(MenuItem menuItem);
    Task DeleteAsync(MenuItem menuItem);
    Task<bool> ExistsAsync(int id);
}
