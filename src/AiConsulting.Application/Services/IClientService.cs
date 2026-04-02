using AiConsulting.Application.DTOs;
using AiConsulting.Application.DTOs.Clients;

namespace AiConsulting.Application.Services;

public interface IClientService
{
    Task<PagedResult<ClientDto>> GetClientsAsync(ClientFilterDto filter);
    Task<ClientDto?> GetClientByIdAsync(Guid id);
    Task<ClientDto> CreateClientAsync(CreateClientDto dto);
    Task<ClientDto> UpdateClientAsync(Guid id, UpdateClientDto dto);
    Task ArchiveClientAsync(Guid id);
}
