using RestaurantOrderAPI.Application.DTOs;
using RestaurantOrderAPI.Application.Helpers;
using RestaurantOrderAPI.Application.Interfaces;
using RestaurantOrderAPI.Domain.Interfaces;

namespace RestaurantOrderAPI.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _uow;
    private readonly ITokenService _tokenService;

    public AuthService(IUnitOfWork uow, ITokenService tokenService)
    {
        _uow = uow;
        _tokenService = tokenService;
    }

    public async Task<TokenResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _uow.Users.GetByUsernameAsync(request.Username);

        if (user is null || !user.IsActive || !PasswordHelper.Verify(request.Password, user.PasswordHash))
            return null;

        var token = _tokenService.GenerateToken(user.Username, user.Role);
        return new TokenResponseDto(token, user.Role, user.Username);
    }
}
