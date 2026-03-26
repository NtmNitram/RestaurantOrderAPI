using RestaurantOrderAPI.Application.DTOs;

namespace RestaurantOrderAPI.Application.Interfaces;

public interface IClientService
{
    Task<IEnumerable<ClientResponseDto>> GetAllAsync();
    Task<ClientResponseDto> GetByIdAsync(int id);
    Task<ClientResponseDto> CreateAsync(CreateClientDto dto);
    Task<ClientResponseDto> UpdateAsync(int id, UpdateClientDto dto);
    Task DeleteAsync(int id);
}
