using Microsoft.EntityFrameworkCore;
using TicketSystemVentura.Domain.Entities;

namespace TicketSystemVentura.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    // DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Response> Responses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de la relación muchos a muchos User-Role
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany()
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany()
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración User - Roles (muchos a muchos)
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRole>();

        // Configuración User - Tickets (uno a muchos)
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tickets)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuración Ticket - Responses (uno a muchos)
        modelBuilder.Entity<Response>()
            .HasOne(r => r.Ticket)
            .WithMany(t => t.Responses)
            .HasForeignKey(r => r.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración User - Responses (uno a muchos)
        modelBuilder.Entity<Response>()
            .HasOne(r => r.Responder)
            .WithMany(u => u.Responses)
            .HasForeignKey(r => r.ResponderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices para mejorar el rendimiento
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Role>()
            .HasIndex(r => r.RoleName)
            .IsUnique();

        modelBuilder.Entity<Ticket>()
            .HasIndex(t => t.Status);
    }
}