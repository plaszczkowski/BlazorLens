using BlazorLens.Application.Interfaces;
using BlazorLens.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using BlazorLens.Application.Interfaces;

namespace BlazorLens.Infrastructure.Data;

/// <summary>
/// Application database context.
/// Implements IQueryDbContext for query handlers (Dependency Inversion).
/// Compliance: ARCH-001 (Clean Architecture), ARCH-002 (Separation of Concerns)
/// </summary>
public class ApplicationDbContext : DbContext, Application.Interfaces.IQueryDbContext
{
    /// <summary>
    /// Initializes a new instance of ApplicationDbContext.
    /// </summary>
    /// <param name="options">Database context options</param>
    /// <exception cref="ArgumentNullException">When options is null</exception>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(options);
    }

    /// <summary>
    /// DbSet for Dashboard entities.
    /// </summary>
    public DbSet<Dashboard> Dashboards => Set<Dashboard>();

    /// <summary>
    /// DbSet for DashboardComponent entities.
    /// </summary>
    public DbSet<DashboardComponent> DashboardComponents => Set<DashboardComponent>();

    /// <summary>
    /// Executes a raw SQL query and returns results.
    /// Compliance: SEC-003 (Input Validation) - use parameterized queries
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="sql">SQL query with parameters</param>
    /// <param name="parameters">Query parameters</param>
    /// <returns>Queryable result set</returns>
    public IQueryable<T> SqlQuery<T>(string sql, params object[] parameters) where T : class
    {
        // Guard clauses - CCP-005
        ArgumentNullException.ThrowIfNull(sql);

        return Set<T>().FromSqlRaw(sql, parameters);
    }

    /// <summary>
    /// Configures entity mappings using FluentAPI.
    /// Compliance: ARCH-002 (Separation of Concerns) - configurations in separate files
    /// </summary>
    /// <param name="modelBuilder">Model builder instance</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);

        // Apply all configurations from current assembly
        // This automatically discovers all IEntityTypeConfiguration<T> implementations
        // Compliance: CON-001 (Change Localization), CCP-001 (DRY)
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Saves changes and dispatches domain events.
    /// Override for future domain event dispatching implementation.
    /// Compliance: ARCH-004 (Event-Driven Design)
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of state entries written to the database</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // TODO: In future, dispatch domain events here before saving
        // 1. Get all entities with domain events
        // 2. Dispatch events via IDomainEventDispatcher
        // 3. Clear events from entities
        // 4. Save changes

        return await base.SaveChangesAsync(cancellationToken);
    }
}