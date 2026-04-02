using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;
using AiConsulting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Repositories;

public class ContactRequestRepository : IContactRequestRepository
{
    private readonly AiConsultingDbContext _context;

    public ContactRequestRepository(AiConsultingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ContactRequest contactRequest)
    {
        await _context.ContactRequests.AddAsync(contactRequest);
        await _context.SaveChangesAsync();
    }

    public async Task<ContactRequest?> GetByIdAsync(Guid id)
    {
        return await _context.ContactRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(cr => cr.Id == id);
    }
}
