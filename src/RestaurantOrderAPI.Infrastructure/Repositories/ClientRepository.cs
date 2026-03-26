using Microsoft.EntityFrameworkCore;
using RestaurantOrderAPI.Domain.Entities;
using RestaurantOrderAPI.Domain.Interfaces;
using RestaurantOrderAPI.Infrastructure.Data;

namespace RestaurantOrderAPI.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Client>> GetAllAsync() =>
        await _context.Clients.OrderBy(c => c.LocalNumber).ToListAsync();

    public async Task<Client?> GetByIdAsync(int id) =>
        await _context.Clients.FindAsync(id);

    public async Task<Client> AddAsync(Client client)
    {
        await _context.Clients.AddAsync(client);
        return client;
    }

    public Task UpdateAsync(Client client)
    {
        _context.Clients.Update(client);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Client client)
    {
        _context.Clients.Remove(client);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id) =>
        await _context.Clients.AnyAsync(c => c.Id == id);
}
