using AiConsulting.Application.DTOs;
using AiConsulting.Application.DTOs.Clients;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;

namespace AiConsulting.Infrastructure.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<PagedResult<ClientDto>> GetClientsAsync(ClientFilterDto filter)
    {
        var (items, totalCount) = await _clientRepository.GetPagedAsync(
            filter.Page, filter.PageSize, filter.Search, filter.IncludeArchived);

        return new PagedResult<ClientDto>
        {
            Items = items.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<ClientDto?> GetClientByIdAsync(Guid id)
    {
        var client = await _clientRepository.GetByIdAsync(id);
        return client is null ? null : MapToDto(client);
    }

    public async Task<ClientDto> CreateClientAsync(CreateClientDto dto)
    {
        var client = new Client
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Company = dto.Company,
            Sector = dto.Sector,
            Email = dto.Email,
            Phone = dto.Phone,
            Notes = dto.Notes,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _clientRepository.AddAsync(client);
        return MapToDto(client);
    }

    public async Task<ClientDto> UpdateClientAsync(Guid id, UpdateClientDto dto)
    {
        var client = await _clientRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Client with id {id} not found.");

        client.Name = dto.Name;
        client.Company = dto.Company;
        client.Sector = dto.Sector;
        client.Email = dto.Email;
        client.Phone = dto.Phone;
        client.Notes = dto.Notes;
        client.UpdatedAt = DateTime.UtcNow;

        await _clientRepository.UpdateAsync(client);
        return MapToDto(client);
    }

    public async Task ArchiveClientAsync(Guid id)
    {
        var client = await _clientRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Client with id {id} not found.");

        client.IsArchived = true;
        client.UpdatedAt = DateTime.UtcNow;

        await _clientRepository.UpdateAsync(client);
    }

    private static ClientDto MapToDto(Client client) => new()
    {
        Id = client.Id,
        Name = client.Name,
        Company = client.Company,
        Sector = client.Sector,
        Email = client.Email,
        Phone = client.Phone,
        Notes = client.Notes,
        IsArchived = client.IsArchived,
        CreatedAt = client.CreatedAt
    };
}
