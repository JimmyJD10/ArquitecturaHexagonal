using TicketSystemVentura.Application.DTOs.Request;
using TicketSystemVentura.Application.DTOs.Response;

namespace TicketSystemVentura.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterUserRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<UserResponse> GetUserByIdAsync(Guid userId);
}