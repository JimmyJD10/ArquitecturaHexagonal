using Microsoft.EntityFrameworkCore;
using TicketSystemVentura.Domain.Entities;
using TicketSystemVentura.Domain.Interfaces;
using TicketSystemVentura.Infrastructure.Data;

namespace TicketSystemVentura.Infrastructure.Repositories;

public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
{
    public TicketRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(t => t.User)
            .Include(t => t.Responses)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByStatusAsync(string status)
    {
        return await _dbSet
            .Include(t => t.User)
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<Ticket?> GetTicketWithResponsesAsync(Guid ticketId)
    {
        return await _dbSet
            .Include(t => t.User)
            .Include(t => t.Responses)
            .ThenInclude(r => r.Responder)
            .FirstOrDefaultAsync(t => t.TicketId == ticketId);
    }

    public async Task<IEnumerable<Ticket>> GetOpenTicketsAsync()
    {
        return await _dbSet
            .Include(t => t.User)
            .Where(t => t.Status == "abierto" || t.Status == "en_proceso")
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
}