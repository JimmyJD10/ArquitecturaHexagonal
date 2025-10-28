using System.ComponentModel.DataAnnotations;

namespace TicketSystemVentura.Application.DTOs.Request;

public class CreateTicketRequest
{
    [Required(ErrorMessage = "El título es requerido")]
    [StringLength(255, ErrorMessage = "El título no puede exceder los 255 caracteres")]
    public string Title { get; set; } = string.Empty;

    [StringLength(5000, ErrorMessage = "La descripción no puede exceder los 5000 caracteres")]
    public string? Description { get; set; }
}