using RestaurantOrderAPI.Application.DTOs;
using RestaurantOrderAPI.Application.Interfaces;
using RestaurantOrderAPI.Domain.Entities;
using RestaurantOrderAPI.Domain.Interfaces;

namespace RestaurantOrderAPI.Application.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IUnitOfWork _unitOfWork;

    public MenuItemService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MenuItemResponseDto>> GetAllAsync()
    {
        var items = await _unitOfWork.MenuItems.GetAllAsync();
        return items.Select(MapToResponse);
    }

    public async Task<IEnumerable<MenuItemResponseDto>> GetAvailableAsync()
    {
        var items = await _unitOfWork.MenuItems.GetAvailableAsync();
        return items.Select(MapToResponse);
    }

    public async Task<MenuItemResponseDto> GetByIdAsync(int id)
    {
        var item = await _unitOfWork.MenuItems.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Platillo con ID {id} no encontrado.");
        return MapToResponse(item);
    }

    public async Task<MenuItemResponseDto> CreateAsync(CreateMenuItemDto dto)
    {
        if (dto.Precio <= 0)
            throw new ArgumentException("El precio debe ser mayor a cero.");

        var item = new MenuItem
        {
            Name = dto.Nombre,
            Description = dto.Descripcion,
            Price = dto.Precio,
            IsAvailable = true
        };
        var created = await _unitOfWork.MenuItems.AddAsync(item);
        await _unitOfWork.SaveChangesAsync();
        return MapToResponse(created);
    }

    public async Task<MenuItemResponseDto> UpdateAsync(int id, UpdateMenuItemDto dto)
    {
        if (dto.Precio <= 0)
            throw new ArgumentException("El precio debe ser mayor a cero.");

        var item = await _unitOfWork.MenuItems.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Platillo con ID {id} no encontrado.");

        item.Name = dto.Nombre;
        item.Description = dto.Descripcion;
        item.Price = dto.Precio;
        item.IsAvailable = dto.Disponible;

        await _unitOfWork.MenuItems.UpdateAsync(item);
        await _unitOfWork.SaveChangesAsync();
        return MapToResponse(item);
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _unitOfWork.MenuItems.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Platillo con ID {id} no encontrado.");
        await _unitOfWork.MenuItems.DeleteAsync(item);
        await _unitOfWork.SaveChangesAsync();
    }

    private static MenuItemResponseDto MapToResponse(MenuItem m) =>
        new(m.Id, m.Name, m.Description, m.Price, m.IsAvailable);
}
