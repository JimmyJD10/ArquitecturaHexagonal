using Microsoft.EntityFrameworkCore;
using TicketSystemVentura.Domain.Entities;
using TicketSystemVentura.Domain.Interfaces;
using TicketSystemVentura.Infrastructure.Data;

namespace TicketSystemVentura.Infrastructure.Repositories;

public class ResponseRepository : GenericRepository<Response>, IResponseRepository
{
    public ResponseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Response>> GetResponsesByTicketIdAsync(Guid ticketId)
    {
        return await _dbSet
            .Include(r => r.Responder)
            .Where(r => r.TicketId == ticketId)
            .OrderBy(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Response>> GetResponsesByResponderIdAsync(Guid responderId)
    {
        return await _dbSet
            .Include(r => r.Ticket)
            .Where(r => r.ResponderId == responderId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }
}