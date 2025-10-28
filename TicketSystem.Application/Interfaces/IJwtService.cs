using TicketSystemVentura.Domain.Entities;

namespace TicketSystemVentura.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user, IEnumerable<string> roles);
    Guid? ValidateToken(string token);
}