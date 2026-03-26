using Microsoft.AspNetCore.Mvc;
using RestaurantOrderAPI.Application.DTOs;
using RestaurantOrderAPI.Application.Interfaces;

namespace RestaurantOrderAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>Obtener todos los pedidos</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _orderService.GetAllAsync());

    /// <summary>Obtener un pedido con todo el detalle</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) =>
        Ok(await _orderService.GetByIdAsync(id));

    /// <summary>Obtener todos los pedidos de un cliente</summary>
    [HttpGet("client/{clientId:int}")]
    public async Task<IActionResult> GetByClient(int clientId) =>
        Ok(await _orderService.GetByClientIdAsync(clientId));

    /// <summary>
    /// Obtener el resumen del día agrupado por cliente con el total a cobrar.
    /// Si no se proporciona fecha, se devuelve el resumen de hoy.
    /// </summary>
    [HttpGet("summary/daily")]
    public async Task<IActionResult> GetDailySummary([FromQuery] DateTime? date)
    {
        var targetDate = date ?? DateTime.Now;
        return Ok(await _orderService.GetDailySummaryAsync(targetDate));
    }

    /// <summary>Crear un nuevo pedido con uno o más artículos</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
    {
        var created = await _orderService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Cambiar el estado del pedido. Transiciones válidas:
    /// Pendiente → Entregado, Pendiente → Cancelado, Entregado → Cancelado
    /// </summary>
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeOrderStatusDto dto) =>
        Ok(await _orderService.ChangeStatusAsync(id, dto));

    /// <summary>Eliminar un pedido</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _orderService.DeleteAsync(id);
        return NoContent();
    }
}
