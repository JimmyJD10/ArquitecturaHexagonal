using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystemVentura.Domain.Entities;

[Table("tickets")]
public class Ticket
{
    [Key]
    [Column("ticket_id")]
    public Guid TicketId { get; set; }

    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "abierto"; // abierto, en_proceso, cerrado

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("closed_at")]
    public DateTime? ClosedAt { get; set; }

    // Relaciones
    public User User { get; set; } = null!;
    public ICollection<Response> Responses { get; set; } = new List<Response>();
}