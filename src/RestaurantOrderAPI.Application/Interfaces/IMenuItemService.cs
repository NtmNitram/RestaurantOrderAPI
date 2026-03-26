using RestaurantOrderAPI.Application.DTOs;

namespace RestaurantOrderAPI.Application.Interfaces;

public interface IMenuItemService
{
    Task<IEnumerable<MenuItemResponseDto>> GetAllAsync();
    Task<IEnumerable<MenuItemResponseDto>> GetAvailableAsync();
    Task<MenuItemResponseDto> GetByIdAsync(int id);
    Task<MenuItemResponseDto> CreateAsync(CreateMenuItemDto dto);
    Task<MenuItemResponseDto> UpdateAsync(int id, UpdateMenuItemDto dto);
    Task DeleteAsync(int id);
}
