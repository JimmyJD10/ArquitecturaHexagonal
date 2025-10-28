using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystemVentura.Domain.Entities;

[Table("roles")]
public class Role
{
    [Key]
    [Column("role_id")]
    public Guid RoleId { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("role_name")]
    public string RoleName { get; set; } = string.Empty;

    // Relación muchos a muchos con Users
    public ICollection<User> Users { get; set; } = new List<User>();
}