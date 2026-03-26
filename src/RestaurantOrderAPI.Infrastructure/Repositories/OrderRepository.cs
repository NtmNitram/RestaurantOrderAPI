using Microsoft.EntityFrameworkCore;
using RestaurantOrderAPI.Domain.Entities;
using RestaurantOrderAPI.Domain.Interfaces;
using RestaurantOrderAPI.Infrastructure.Data;

namespace RestaurantOrderAPI.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllAsync() =>
        await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.MenuItem)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

    public async Task<Order?> GetByIdWithDetailsAsync(int id) =>
        await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.MenuItem)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IEnumerable<Order>> GetByClientIdAsync(int clientId) =>
        await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.MenuItem)
            .Where(o => o.ClientId == clientId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

    public async Task<IEnumerable<Order>> GetDailySummaryAsync(DateTime date)
    {
        var start = date.Date;
        var end = start.AddDays(1);

        return await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.MenuItem)
            .Where(o => o.OrderDate >= start && o.OrderDate < end)
            .OrderBy(o => o.Client.LocalNumber)
            .ThenBy(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order> AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        return order;
    }

    public Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Order order)
    {
        _context.Orders.Remove(order);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id) =>
        await _context.Orders.AnyAsync(o => o.Id == id);
}
