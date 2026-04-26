using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOrderAPI.Application.DTOs;
using RestaurantOrderAPI.Application.Interfaces;

namespace RestaurantOrderAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class MenuItemsController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;

    public MenuItemsController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    /// <summary>Obtener todos los platillos del menú</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _menuItemService.GetAllAsync());

    /// <summary>Obtener solo los platillos disponibles</summary>
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable() =>
        Ok(await _menuItemService.GetAvailableAsync());

    /// <summary>Obtener un platillo por ID</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) =>
        Ok(await _menuItemService.GetByIdAsync(id));

    /// <summary>Agregar un nuevo platillo al menú</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMenuItemDto dto)
    {
        var created = await _menuItemService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Actualizar un platillo (nombre, precio, disponibilidad)</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMenuItemDto dto) =>
        Ok(await _menuItemService.UpdateAsync(id, dto));

    /// <summary>Eliminar un platillo</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _menuItemService.DeleteAsync(id);
        return NoContent();
    }
}
