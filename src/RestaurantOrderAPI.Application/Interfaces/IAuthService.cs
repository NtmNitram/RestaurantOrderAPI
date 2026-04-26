using RestaurantOrderAPI.Application.DTOs;

namespace RestaurantOrderAPI.Application.Interfaces;

public interface IAuthService
{
    Task<TokenResponseDto?> LoginAsync(LoginRequestDto request);
}
