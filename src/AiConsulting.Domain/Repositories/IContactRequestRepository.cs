using AiConsulting.Domain.Entities;

namespace AiConsulting.Domain.Repositories;

public interface IContactRequestRepository
{
    Task AddAsync(ContactRequest contactRequest);
    Task<ContactRequest?> GetByIdAsync(Guid id);
}
