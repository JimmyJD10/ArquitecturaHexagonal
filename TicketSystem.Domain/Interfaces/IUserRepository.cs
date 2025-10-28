using TicketSystemVentura.Domain.Entities;

namespace TicketSystemVentura.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetUserWithRolesAsync(Guid userId);
    Task<bool> UserExistsByUsernameAsync(string username);
    Task<bool> UserExistsByEmailAsync(string email);
}