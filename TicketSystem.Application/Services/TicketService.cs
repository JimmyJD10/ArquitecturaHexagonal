using TicketSystemVentura.Application.DTOs.Request;
using TicketSystemVentura.Application.DTOs.Response;
using TicketSystemVentura.Application.Interfaces;
using TicketSystemVentura.Domain.Entities;
using TicketSystemVentura.Domain.Interfaces;

namespace TicketSystemVentura.Application.Services;

public class TicketService : ITicketService
{
    private readonly IUnitOfWork _unitOfWork;

    public TicketService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TicketResponse> CreateTicketAsync(Guid userId, CreateTicketRequest request)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("Usuario no encontrado");
        }

        var ticket = new Ticket
        {
            TicketId = Guid.NewGuid(),
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            Status = "abierto",
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Tickets.AddAsync(ticket);
        await _unitOfWork.SaveChangesAsync();

        return new TicketResponse
        {
            TicketId = ticket.TicketId,
            UserId = ticket.UserId,
            Username = user.Username,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status,
            CreatedAt = ticket.CreatedAt,
            ClosedAt = ticket.ClosedAt,
            Responses = new List<ResponseDto>()
        };
    }

    public async Task<TicketResponse?> GetTicketByIdAsync(Guid ticketId)
    {
        var ticket = await _unitOfWork.Tickets.GetTicketWithResponsesAsync(ticketId);
        
        if (ticket == null)
        {
            return null;
        }

        return new TicketResponse
        {
            TicketId = ticket.TicketId,
            UserId = ticket.UserId,
            Username = ticket.User.Username,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status,
            CreatedAt = ticket.CreatedAt,
            ClosedAt = ticket.ClosedAt,
            Responses = ticket.Responses.Select(r => new ResponseDto
            {
                ResponseId = r.ResponseId,
                TicketId = r.TicketId,
                ResponderId = r.ResponderId,
                ResponderUsername = r.Responder.Username,
                Message = r.Message,
                CreatedAt = r.CreatedAt
            }).ToList()
        };
    }

    public async Task<IEnumerable<TicketResponse>> GetAllTicketsAsync()
    {
        var tickets = await _unitOfWork.Tickets.GetAllAsync();
        
        var ticketResponses = new List<TicketResponse>();
        
        foreach (var ticket in tickets)
        {
            var ticketWithDetails = await _unitOfWork.Tickets.GetTicketWithResponsesAsync(ticket.TicketId);
            if (ticketWithDetails != null)
            {
                ticketResponses.Add(new TicketResponse
                {
                    TicketId = ticketWithDetails.TicketId,
                    UserId = ticketWithDetails.UserId,
                    Username = ticketWithDetails.User.Username,
                    Title = ticketWithDetails.Title,
                    Description = ticketWithDetails.Description,
                    Status = ticketWithDetails.Status,
                    CreatedAt = ticketWithDetails.CreatedAt,
                    ClosedAt = ticketWithDetails.ClosedAt,
                    Responses = ticketWithDetails.Responses.Select(r => new ResponseDto
                    {
                        ResponseId = r.ResponseId,
                        TicketId = r.TicketId,
                        ResponderId = r.ResponderId,
                        ResponderUsername = r.Responder.Username,
                        Message = r.Message,
                        CreatedAt = r.CreatedAt
                    }).ToList()
                });
            }
        }
        
        return ticketResponses;
    }

    public async Task<IEnumerable<TicketResponse>> GetTicketsByUserIdAsync(Guid userId)
    {
        var tickets = await _unitOfWork.Tickets.GetTicketsByUserIdAsync(userId);
        
        return tickets.Select(ticket => new TicketResponse
        {
            TicketId = ticket.TicketId,
            UserId = ticket.UserId,
            Username = ticket.User.Username,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status,
            CreatedAt = ticket.CreatedAt,
            ClosedAt = ticket.ClosedAt,
            Responses = ticket.Responses.Select(r => new ResponseDto
            {
                ResponseId = r.ResponseId,
                TicketId = r.TicketId,
                ResponderId = r.ResponderId,
                ResponderUsername = r.Responder.Username,
                Message = r.Message,
                CreatedAt = r.CreatedAt
            }).ToList()
        }).ToList();
    }

    public async Task<IEnumerable<TicketResponse>> GetTicketsByStatusAsync(string status)
    {
        var tickets = await _unitOfWork.Tickets.GetTicketsByStatusAsync(status);
        
        return tickets.Select(ticket => new TicketResponse
        {
            TicketId = ticket.TicketId,
            UserId = ticket.UserId,
            Username = ticket.User.Username,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status,
            CreatedAt = ticket.CreatedAt,
            ClosedAt = ticket.ClosedAt,
            Responses = ticket.Responses.Select(r => new ResponseDto
            {
                ResponseId = r.ResponseId,
                TicketId = r.TicketId,
                ResponderId = r.ResponderId,
                ResponderUsername = r.Responder.Username,
                Message = r.Message,
                CreatedAt = r.CreatedAt
            }).ToList()
        }).ToList();
    }

    public async Task<TicketResponse> UpdateTicketStatusAsync(Guid ticketId, UpdateTicketStatusRequest request)
    {
        var ticket = await _unitOfWork.Tickets.GetTicketWithResponsesAsync(ticketId);
        
        if (ticket == null)
        {
            throw new KeyNotFoundException("Ticket no encontrado");
        }

        ticket.Status = request.Status;
        
        if (request.Status == "cerrado")
        {
            ticket.ClosedAt = DateTime.UtcNow;
        }

        await _unitOfWork.Tickets.UpdateAsync(ticket);
        await _unitOfWork.SaveChangesAsync();

        return new TicketResponse
        {
            TicketId = ticket.TicketId,
            UserId = ticket.UserId,
            Username = ticket.User.Username,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status,
            CreatedAt = ticket.CreatedAt,
            ClosedAt = ticket.ClosedAt,
            Responses = ticket.Responses.Select(r => new ResponseDto
            {
                ResponseId = r.ResponseId,
                TicketId = r.TicketId,
                ResponderId = r.ResponderId,
                ResponderUsername = r.Responder.Username,
                Message = r.Message,
                CreatedAt = r.CreatedAt
            }).ToList()
        };
    }

    public async Task<bool> DeleteTicketAsync(Guid ticketId)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);
        
        if (ticket == null)
        {
            return false;
        }

        await _unitOfWork.Tickets.DeleteAsync(ticket);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}