using Microsoft.AspNetCore.Mvc;
using RestaurantOrderAPI.Application.DTOs;
using RestaurantOrderAPI.Application.Interfaces;

namespace RestaurantOrderAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>Iniciar sesión. Devuelve JWT y rol del usuario.</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (result is null)
            return Unauthorized(new { message = "Usuario o contraseña incorrectos" });

        return Ok(result);
    }
}
