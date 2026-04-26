namespace RestaurantOrderAPI.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IClientRepository Clients { get; }
    IMenuItemRepository MenuItems { get; }
    IOrderRepository Orders { get; }
    IUserRepository Users { get; }
    Task<int> SaveChangesAsync();
}
