using Microsoft.EntityFrameworkCore;
using RestaurantOrderAPI.Domain.Entities;
using RestaurantOrderAPI.Domain.Interfaces;
using RestaurantOrderAPI.Infrastructure.Data;

namespace RestaurantOrderAPI.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
}
