using System.ComponentModel.DataAnnotations;

namespace TicketSystemVentura.Application.DTOs.Request;

public class RegisterUserRequest
{
    [Required(ErrorMessage = "El nombre de usuario es requerido")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 100 caracteres")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; set; } = string.Empty;
}