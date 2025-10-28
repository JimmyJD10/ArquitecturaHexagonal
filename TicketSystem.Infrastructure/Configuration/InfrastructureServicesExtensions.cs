using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketSystemVentura.Domain.Interfaces;
using TicketSystemVentura.Infrastructure.Data;
using TicketSystemVentura.Infrastructure.Repositories;

namespace TicketSystemVentura.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Configuración de la base de datos
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        });

        // Registrar repositorios
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IResponseRepository, ResponseRepository>();

        // Registrar UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}