using RestaurantOrderAPI.Domain.Entities;

namespace RestaurantOrderAPI.Domain.Interfaces;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllAsync();
    Task<Client?> GetByIdAsync(int id);
    Task<Client> AddAsync(Client client);
    Task UpdateAsync(Client client);
    Task DeleteAsync(Client client);
    Task<bool> ExistsAsync(int id);
}
