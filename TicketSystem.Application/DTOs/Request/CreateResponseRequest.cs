using System.ComponentModel.DataAnnotations;

namespace TicketSystemVentura.Application.DTOs.Request;

public class CreateResponseRequest
{
    [Required(ErrorMessage = "El mensaje es requerido")]
    [StringLength(5000, ErrorMessage = "El mensaje no puede exceder los 5000 caracteres")]
    public string Message { get; set; } = string.Empty;
}