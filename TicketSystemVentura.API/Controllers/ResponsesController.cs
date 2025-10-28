using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketSystemVentura.Application.DTOs.Request;
using TicketSystemVentura.Application.Interfaces;

namespace TicketSystemVentura.API.Controllers;

[ApiController]
[Route("api/tickets/{ticketId}/[controller]")]
[Authorize]
public class ResponsesController : ControllerBase
{
    private readonly IResponseService _responseService;

    public ResponsesController(IResponseService responseService)
    {
        _responseService = responseService;
    }

    /// <summary>
    /// Crear una respuesta a un ticket
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateResponse(Guid ticketId, [FromBody] CreateResponseRequest request)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            var response = await _responseService.CreateResponseAsync(ticketId, userId, request);
            return CreatedAtAction(nameof(GetResponsesByTicket), new { ticketId }, response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener todas las respuestas de un ticket
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetResponsesByTicket(Guid ticketId)
    {
        try
        {
            var responses = await _responseService.GetResponsesByTicketIdAsync(ticketId);
            return Ok(responses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }
}