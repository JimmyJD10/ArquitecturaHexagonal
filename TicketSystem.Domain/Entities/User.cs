using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystemVentura.Domain.Entities;

[Table("users")]
public class User
{
    [Key]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(150)]
    [Column("email")]
    public string? Email { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relaciones
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<Response> Responses { get; set; } = new List<Response>();
}