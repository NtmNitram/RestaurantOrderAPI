using Microsoft.EntityFrameworkCore;
using RestaurantOrderAPI.Domain.Entities;
using RestaurantOrderAPI.Domain.Interfaces;
using RestaurantOrderAPI.Infrastructure.Data;

namespace RestaurantOrderAPI.Infrastructure.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly AppDbContext _context;

    public MenuItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync() =>
        await _context.MenuItems.OrderBy(m => m.Name).ToListAsync();

    public async Task<IEnumerable<MenuItem>> GetAvailableAsync() =>
        await _context.MenuItems
            .Where(m => m.IsAvailable)
            .OrderBy(m => m.Name)
            .ToListAsync();

    public async Task<MenuItem?> GetByIdAsync(int id) =>
        await _context.MenuItems.FindAsync(id);

    public async Task<MenuItem> AddAsync(MenuItem menuItem)
    {
        await _context.MenuItems.AddAsync(menuItem);
        return menuItem;
    }

    public Task UpdateAsync(MenuItem menuItem)
    {
        _context.MenuItems.Update(menuItem);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(MenuItem menuItem)
    {
        _context.MenuItems.Remove(menuItem);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id) =>
        await _context.MenuItems.AnyAsync(m => m.Id == id);
}
