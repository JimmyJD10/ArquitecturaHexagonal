using TicketSystemVentura.Application.DTOs.Request;
using TicketSystemVentura.Application.DTOs.Response;

namespace TicketSystemVentura.Application.Interfaces;

public interface IResponseService
{
    Task<ResponseDto> CreateResponseAsync(Guid ticketId, Guid responderId, CreateResponseRequest request);
    Task<IEnumerable<ResponseDto>> GetResponsesByTicketIdAsync(Guid ticketId);
}