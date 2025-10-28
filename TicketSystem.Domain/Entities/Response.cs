using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystemVentura.Domain.Entities;

[Table("responses")]
public class Response
{
    [Key]
    [Column("response_id")]
    public Guid ResponseId { get; set; }

    [Required]
    [Column("ticket_id")]
    public Guid TicketId { get; set; }

    [Required]
    [Column("responder_id")]
    public Guid ResponderId { get; set; }

    [Required]
    [Column("message")]
    public string Message { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relaciones
    public Ticket Ticket { get; set; } = null!;
    public User Responder { get; set; } = null!;
}