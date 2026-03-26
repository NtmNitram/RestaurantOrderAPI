using RestaurantOrderAPI.Application.DTOs;
using RestaurantOrderAPI.Application.Interfaces;
using RestaurantOrderAPI.Domain.Entities;
using RestaurantOrderAPI.Domain.Interfaces;

namespace RestaurantOrderAPI.Application.Services;

public class ClientService : IClientService
{
    private readonly IUnitOfWork _unitOfWork;

    public ClientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ClientResponseDto>> GetAllAsync()
    {
        var clients = await _unitOfWork.Clients.GetAllAsync();
        return clients.Select(MapToResponse);
    }

    public async Task<ClientResponseDto> GetByIdAsync(int id)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Cliente con ID {id} no encontrado.");
        return MapToResponse(client);
    }

    public async Task<ClientResponseDto> CreateAsync(CreateClientDto dto)
    {
        var client = new Client
        {
            Name = dto.Nombre,
            LocalNumber = dto.NumeroLocal,
            PhoneNumber = dto.Telefono,
            IsActive = true
        };
        var created = await _unitOfWork.Clients.AddAsync(client);
        await _unitOfWork.SaveChangesAsync();
        return MapToResponse(created);
    }

    public async Task<ClientResponseDto> UpdateAsync(int id, UpdateClientDto dto)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Cliente con ID {id} no encontrado.");

        client.Name = dto.Nombre;
        client.LocalNumber = dto.NumeroLocal;
        client.PhoneNumber = dto.Telefono;
        client.IsActive = dto.Activo;

        await _unitOfWork.Clients.UpdateAsync(client);
        await _unitOfWork.SaveChangesAsync();
        return MapToResponse(client);
    }

    public async Task DeleteAsync(int id)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Cliente con ID {id} no encontrado.");
        await _unitOfWork.Clients.DeleteAsync(client);
        await _unitOfWork.SaveChangesAsync();
    }

    private static ClientResponseDto MapToResponse(Client c) =>
        new(c.Id, c.Name, c.LocalNumber, c.PhoneNumber, c.IsActive);
}
