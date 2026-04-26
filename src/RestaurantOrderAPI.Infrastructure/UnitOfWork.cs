using RestaurantOrderAPI.Domain.Interfaces;
using RestaurantOrderAPI.Infrastructure.Data;
using RestaurantOrderAPI.Infrastructure.Repositories;

namespace RestaurantOrderAPI.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Clients = new ClientRepository(context);
        MenuItems = new MenuItemRepository(context);
        Orders = new OrderRepository(context);
        Users = new UserRepository(context);
    }

    public IClientRepository Clients { get; }
    public IMenuItemRepository MenuItems { get; }
    public IOrderRepository Orders { get; }
    public IUserRepository Users { get; }

    public async Task<int> SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
