using Microsoft.EntityFrameworkCore;
using TicketSystemVentura.Domain.Entities;
using TicketSystemVentura.Domain.Interfaces;
using TicketSystemVentura.Infrastructure.Data;

namespace TicketSystemVentura.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserWithRolesAsync(Guid userId)
    {
        return await _dbSet
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<bool> UserExistsByUsernameAsync(string username)
    {
        return await _dbSet.AnyAsync(u => u.Username == username);
    }

    public async Task<bool> UserExistsByEmailAsync(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email == email);
    }
}