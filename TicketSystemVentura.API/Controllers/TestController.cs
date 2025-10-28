/*
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystemVentura.Infrastructure.Data;

namespace TicketSystemVentura.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TestController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("test-connection")]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            // Intenta conectar a la base de datos
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (canConnect)
            {
                // Cuenta cuántos usuarios hay
                var userCount = await _context.Users.CountAsync();
                var roleCount = await _context.Roles.CountAsync();
                var ticketCount = await _context.Tickets.CountAsync();
                
                return Ok(new
                {
                    message = "Conexión exitosa a la base de datos",
                    database = "SoporteDB",
                    users = userCount,
                    roles = roleCount,
                    tickets = ticketCount
                });
            }
            
            return StatusCode(500, new { message = "No se pudo conectar a la base de datos" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Error al conectar a la base de datos",
                error = ex.Message,
                innerError = ex.InnerException?.Message
            });
        }
    }
}

*/