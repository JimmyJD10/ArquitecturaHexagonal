using Microsoft.EntityFrameworkCore.Storage;
using TicketSystemVentura.Domain.Interfaces;
using TicketSystemVentura.Infrastructure.Data;

namespace TicketSystemVentura.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public IUserRepository Users { get; }
    public IRoleRepository Roles { get; }
    public ITicketRepository Tickets { get; }
    public IResponseRepository Responses { get; }

    public UnitOfWork(
        ApplicationDbContext context,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        ITicketRepository ticketRepository,
        IResponseRepository responseRepository)
    {
        _context = context;
        Users = userRepository;
        Roles = roleRepository;
        Tickets = ticketRepository;
        Responses = responseRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}