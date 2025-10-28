using TicketSystemVentura.Application.DTOs.Request;
using TicketSystemVentura.Application.DTOs.Response;
using TicketSystemVentura.Application.Interfaces;
using TicketSystemVentura.Domain.Entities;
using TicketSystemVentura.Domain.Interfaces;

namespace TicketSystemVentura.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterUserRequest request)
    {
        // Verificar si el usuario ya existe
        if (await _unitOfWork.Users.UserExistsByUsernameAsync(request.Username))
        {
            throw new InvalidOperationException("El nombre de usuario ya existe");
        }

        if (await _unitOfWork.Users.UserExistsByEmailAsync(request.Email))
        {
            throw new InvalidOperationException("El email ya está registrado");
        }

        // Crear nuevo usuario
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password), // Encriptación
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user);
        // Asignar rol por defecto "User"
        var userRole = await _unitOfWork.Roles.GetByNameAsync("User");
        if (userRole != null)
        {
            user.Roles.Add(userRole);
        }
        await _unitOfWork.SaveChangesAsync();
        // Generar token
        var roles = user.Roles.Select(r => r.RoleName).ToList();
        var token = _jwtService.GenerateToken(user, roles);

        return new AuthResponse
        {
            Token = token,
            Username = user.Username,
            Email = user.Email ?? string.Empty,
            Roles = roles,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };
    }
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(request.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");
        }

        // Generar token
        var roles = user.Roles.Select(r => r.RoleName).ToList();
        var token = _jwtService.GenerateToken(user, roles);

        return new AuthResponse
        {
            Token = token,
            Username = user.Username,
            Email = user.Email ?? string.Empty,
            Roles = roles,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };
    }

    public async Task<UserResponse> GetUserByIdAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetUserWithRolesAsync(userId);

        if (user == null)
        {
            throw new KeyNotFoundException("Usuario no encontrado");
        }

        return new UserResponse
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email ?? string.Empty,
            CreatedAt = user.CreatedAt,
            Roles = user.Roles.Select(r => r.RoleName).ToList()
        };
    }
}