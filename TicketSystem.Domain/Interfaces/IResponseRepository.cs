using TicketSystemVentura.Domain.Entities;

namespace TicketSystemVentura.Domain.Interfaces;

public interface IResponseRepository : IGenericRepository<Response>
{
    Task<IEnumerable<Response>> GetResponsesByTicketIdAsync(Guid ticketId);
    Task<IEnumerable<Response>> GetResponsesByResponderIdAsync(Guid responderId);
}