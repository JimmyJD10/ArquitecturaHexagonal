using System.ComponentModel.DataAnnotations;

namespace TicketSystemVentura.Application.DTOs.Request;

public class LoginRequest
{
    [Required(ErrorMessage = "El nombre de usuario es requerido")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    public string Password { get; set; } = string.Empty;
}