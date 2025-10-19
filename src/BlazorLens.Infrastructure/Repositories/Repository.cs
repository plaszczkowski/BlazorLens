using BlazorLens.Application.Interfaces;
using BlazorLens.Infrastructure.Data;
using BlazorLens.Shared.Kernel;
using Microsoft.EntityFrameworkCore;

namespace BlazorLens.Infrastructure.Repositories;

/// <summary>
/// Generic repository implementation for entities.
/// Does NOT manage transactions - use UnitOfWork or DbContext.SaveChangesAsync.
/// Compliance: OOD-001 (Single Responsibility), ARCH-001 (Clean Architecture)
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public class Repository<T> : IRepository<T> where T : Entity<Guid>
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _entities;

    /// <summary>
    /// Initializes a new instance of Repository.
    /// </summary>
    /// <param name="context">Database context</param>
    /// <exception cref="ArgumentNullException">When context is null</exception>
    public Repository(ApplicationDbContext context)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
        _entities = context.Set<T>();
    }

    /// <summary>
    /// Gets an entity by its identifier.
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <returns>Entity if found, null otherwise</returns>
    /// <exception cref="ArgumentException">When id is empty</exception>
    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        // Guard clause - CCP-005
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }

        return await _entities.FindAsync(id);
    }

    /// <summary>
    /// Gets all entities.
    /// WARNING: Use with caution on large datasets. Consider pagination.
    /// </summary>
    /// <returns>List of all entities</returns>
    public virtual async Task<List<T>> GetAllAsync()
    {
        return await _entities.ToListAsync();
    }

    /// <summary>
    /// Adds an entity to the change tracker.
    /// NOTE: Does NOT save to database. Call UnitOfWork.SaveChangesAsync or DbContext.SaveChangesAsync.
    /// Compliance: OOD-001 - Repository only tracks changes, doesn't manage transactions
    /// </summary>
    /// <param name="entity">Entity to add</param>
    /// <exception cref="ArgumentNullException">When entity is null</exception>
    public virtual async Task AddAsync(T entity)
    {
        // Guard clause - CCP-005
        ArgumentNullException.ThrowIfNull(entity);

        await _entities.AddAsync(entity);
        // NOTE: NO SaveChangesAsync here!
        // Transaction management is responsibility of Application Layer
    }

    /// <summary>
    /// Updates an entity in the change tracker.
    /// NOTE: Does NOT save to database. Call UnitOfWork.SaveChangesAsync or DbContext.SaveChangesAsync.
    /// </summary>
    /// <param name="entity">Entity to update</param>
    /// <exception cref="ArgumentNullException">When entity is null</exception>
    public virtual Task UpdateAsync(T entity)
    {
        // Guard clause - CCP-005
        ArgumentNullException.ThrowIfNull(entity);

        _entities.Update(entity);
        // NOTE: NO SaveChangesAsync here!

        return Task.CompletedTask;
    }

    /// <summary>
    /// Removes an entity from the change tracker.
    /// NOTE: Does NOT save to database. Call UnitOfWork.SaveChangesAsync or DbContext.SaveChangesAsync.
    /// </summary>
    /// <param name="entity">Entity to delete</param>
    /// <exception cref="ArgumentNullException">When entity is null</exception>
    public virtual Task DeleteAsync(T entity)
    {
        // Guard clause - CCP-005
        ArgumentNullException.ThrowIfNull(entity);

        _entities.Remove(entity);
        // NOTE: NO SaveChangesAsync here!

        return Task.CompletedTask;
    }
}