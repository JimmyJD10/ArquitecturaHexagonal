using TicketSystemVentura.Application.DTOs.Request;
using TicketSystemVentura.Application.DTOs.Response;
using TicketSystemVentura.Application.Interfaces;
using TicketSystemVentura.Domain.Entities;
using TicketSystemVentura.Domain.Interfaces;

namespace TicketSystemVentura.Application.Services;

public class ResponseService : IResponseService
{
    private readonly IUnitOfWork _unitOfWork;

    public ResponseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDto> CreateResponseAsync(Guid ticketId, Guid responderId, CreateResponseRequest request)
    {
        // Verificar que el ticket existe
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);
        if (ticket == null)
        {
            throw new KeyNotFoundException("Ticket no encontrado");
        }

        // Verificar que el usuario existe
        var responder = await _unitOfWork.Users.GetByIdAsync(responderId);
        if (responder == null)
        {
            throw new KeyNotFoundException("Usuario no encontrado");
        }

        var response = new Response
        {
            ResponseId = Guid.NewGuid(),
            TicketId = ticketId,
            ResponderId = responderId,
            Message = request.Message,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Responses.AddAsync(response);
        await _unitOfWork.SaveChangesAsync();

        return new ResponseDto
        {
            ResponseId = response.ResponseId,
            TicketId = response.TicketId,
            ResponderId = response.ResponderId,
            ResponderUsername = responder.Username,
            Message = response.Message,
            CreatedAt = response.CreatedAt
        };
    }

    public async Task<IEnumerable<ResponseDto>> GetResponsesByTicketIdAsync(Guid ticketId)
    {
        var responses = await _unitOfWork.Responses.GetResponsesByTicketIdAsync(ticketId);

        return responses.Select(r => new ResponseDto
        {
            ResponseId = r.ResponseId,
            TicketId = r.TicketId,
            ResponderId = r.ResponderId,
            ResponderUsername = r.Responder.Username,
            Message = r.Message,
            CreatedAt = r.CreatedAt
        }).ToList();
    }
}