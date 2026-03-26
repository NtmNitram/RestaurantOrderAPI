using Microsoft.AspNetCore.Mvc;
using RestaurantOrderAPI.Application.DTOs;
using RestaurantOrderAPI.Application.Interfaces;

namespace RestaurantOrderAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    /// <summary>Obtener todos los clientes</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _clientService.GetAllAsync());

    /// <summary>Obtener un cliente por ID</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) =>
        Ok(await _clientService.GetByIdAsync(id));

    /// <summary>Registrar un nuevo cliente (local o negocio)</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClientDto dto)
    {
        var created = await _clientService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Actualizar un cliente existente</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClientDto dto) =>
        Ok(await _clientService.UpdateAsync(id, dto));

    /// <summary>Eliminar un cliente</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _clientService.DeleteAsync(id);
        return NoContent();
    }
}
