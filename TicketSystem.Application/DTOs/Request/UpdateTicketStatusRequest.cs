using System.ComponentModel.DataAnnotations;

namespace TicketSystemVentura.Application.DTOs.Request;

public class UpdateTicketStatusRequest
{
    [Required(ErrorMessage = "El estado es requerido")]
    [RegularExpression("^(abierto|en_proceso|cerrado)$", 
        ErrorMessage = "El estado debe ser: abierto, en_proceso o cerrado")]
    public string Status { get; set; } = string.Empty;
}