using TicketSystemVentura.Application.DTOs.Request;
using TicketSystemVentura.Application.DTOs.Response;

namespace TicketSystemVentura.Application.Interfaces;

public interface ITicketService
{
    Task<TicketResponse> CreateTicketAsync(Guid userId, CreateTicketRequest request);
    Task<TicketResponse?> GetTicketByIdAsync(Guid ticketId);
    Task<IEnumerable<TicketResponse>> GetAllTicketsAsync();
    Task<IEnumerable<TicketResponse>> GetTicketsByUserIdAsync(Guid userId);
    Task<IEnumerable<TicketResponse>> GetTicketsByStatusAsync(string status);
    Task<TicketResponse> UpdateTicketStatusAsync(Guid ticketId, UpdateTicketStatusRequest request);
    Task<bool> DeleteTicketAsync(Guid ticketId);
}