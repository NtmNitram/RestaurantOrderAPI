namespace RestaurantOrderAPI.Application.DTOs;

public record LoginRequestDto(string Username, string Password);

public record TokenResponseDto(string Token, string Role, string Username);
