using Microsoft.EntityFrameworkCore;
using TicketSystemVentura.Domain.Entities;
using TicketSystemVentura.Domain.Interfaces;
using TicketSystemVentura.Infrastructure.Data;

namespace TicketSystemVentura.Infrastructure.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetByNameAsync(string roleName)
    {
        return await _dbSet
            .FirstOrDefaultAsync(r => r.RoleName == roleName);
    }

    public async Task<bool> RoleExistsByNameAsync(string roleName)
    {
        return await _dbSet.AnyAsync(r => r.RoleName == roleName);
    }
}