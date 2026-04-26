using RestaurantOrderAPI.Domain.Entities;

namespace RestaurantOrderAPI.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
}
