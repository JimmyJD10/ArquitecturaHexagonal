using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TicketSystemVentura.Infrastructure.Configuration;
using TicketSystemVentura.Application.Interfaces;
using TicketSystemVentura.Application.Services;

namespace TicketSystemVentura.API.Configuration;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar servicios de Infrastructure
        services.AddInfrastructureServices(configuration);

        // Registrar servicios de Application
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<IResponseService, ResponseService>();

        // Configurar JWT Authentication
        services.AddJwtAuthentication(configuration);

        // Configurar Swagger
        services.AddSwaggerConfiguration();

        // Configurar Controllers
        services.AddControllers();

        // Configurar CORS (opcional)
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey ?? throw new InvalidOperationException("JWT SecretKey not configured")))
            };
        });

        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection AddSwaggerConfiguration(
        this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Ticket System API",
                Version = "v1",
                Description = "API para gestión de tickets de soporte",
                Contact = new OpenApiContact
                {
                    Name = "Tu Nombre",
                    Email = "tu@email.com"
                }
            });

            // Configuración para JWT en Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}