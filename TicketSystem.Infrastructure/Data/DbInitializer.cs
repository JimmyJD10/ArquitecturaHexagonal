using Microsoft.EntityFrameworkCore;
using TicketSystemVentura.Domain.Entities;

namespace TicketSystemVentura.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        // Asegurar que la base de datos existe
        await context.Database.EnsureCreatedAsync();

        // Verificar si ya hay roles
        if (await context.Roles.AnyAsync())
        {
            return; // La base de datos ya tiene datos
        }

        // Crear roles
        var roles = new List<Role>
        {
            new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = "Admin"
            },
            new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = "User"
            },
            new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = "Support"
            }
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();

        // Crear usuario administrador de prueba
        var adminUser = new User
        {
            UserId = Guid.NewGuid(),
            Username = "admin",
            Email = "admin@ticketsystem.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"), // Contraseña: Admin123
            CreatedAt = DateTime.UtcNow
        };

        await context.Users.AddAsync(adminUser);
        await context.SaveChangesAsync();

        // Asignar rol Admin al usuario
        var adminRole = roles.First(r => r.RoleName == "Admin");
        var userRole = new UserRole
        {
            UserId = adminUser.UserId,
            RoleId = adminRole.RoleId,
            AssignedAt = DateTime.UtcNow
        };

        await context.UserRoles.AddAsync(userRole);
        await context.SaveChangesAsync();

        // Crear usuario normal de prueba
        var normalUser = new User
        {
            UserId = Guid.NewGuid(),
            Username = "testuser",
            Email = "testuser@ticketsystem.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123"), // Contraseña: Test123
            CreatedAt = DateTime.UtcNow
        };

        await context.Users.AddAsync(normalUser);
        await context.SaveChangesAsync();

        // Asignar rol User al usuario normal
        var normalUserRole = roles.First(r => r.RoleName == "User");
        var userRoleMapping = new UserRole
        {
            UserId = normalUser.UserId,
            RoleId = normalUserRole.RoleId,
            AssignedAt = DateTime.UtcNow
        };

        await context.UserRoles.AddAsync(userRoleMapping);
        await context.SaveChangesAsync();

        Console.WriteLine(" Datos iniciales creados correctamente:");
        Console.WriteLine("   - 3 Roles: Admin, User, Support");
        Console.WriteLine("   - Usuario Admin: admin / Admin123");
        Console.WriteLine("   - Usuario Normal: testuser / Test123");
    }
}