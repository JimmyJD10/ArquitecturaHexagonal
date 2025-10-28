using TicketSystemVentura.Domain.Entities;

namespace TicketSystemVentura.Domain.Interfaces;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<Role?> GetByNameAsync(string roleName);
    Task<bool> RoleExistsByNameAsync(string roleName);
}