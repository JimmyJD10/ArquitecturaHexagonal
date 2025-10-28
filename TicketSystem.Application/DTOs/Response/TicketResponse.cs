namespace TicketSystemVentura.Application.DTOs.Response;

public class TicketResponse
{
    public Guid TicketId { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public List<ResponseDto> Responses { get; set; } = new();
}