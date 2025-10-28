using TicketSystemVentura.Domain.Entities;

namespace TicketSystemVentura.Domain.Interfaces;

public interface ITicketRepository : IGenericRepository<Ticket>
{
    Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(Guid userId);
    Task<IEnumerable<Ticket>> GetTicketsByStatusAsync(string status);
    Task<Ticket?> GetTicketWithResponsesAsync(Guid ticketId);
    Task<IEnumerable<Ticket>> GetOpenTicketsAsync();
}