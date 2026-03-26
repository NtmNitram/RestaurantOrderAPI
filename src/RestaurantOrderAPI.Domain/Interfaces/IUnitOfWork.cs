namespace RestaurantOrderAPI.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IClientRepository Clients { get; }
    IMenuItemRepository MenuItems { get; }
    IOrderRepository Orders { get; }
    Task<int> SaveChangesAsync();
}
