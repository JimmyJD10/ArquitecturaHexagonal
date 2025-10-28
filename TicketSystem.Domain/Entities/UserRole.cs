using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystemVentura.Domain.Entities;

[Table("user_roles")]
public class UserRole
{
    [Column("user_id")]
    public Guid UserId { get; set; }
    
    [Column("role_id")]
    public Guid RoleId { get; set; }

    [Column("assigned_at")]
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    // Navegación
    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}