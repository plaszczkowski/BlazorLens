namespace BlazorLens.Infrastructure.Data;

/// <summary>
/// Application database context.
/// Implements IQueryDbContext for query handlers (Dependency Inversion).
/// Compliance: ARCH-001 (Clean Architecture), ARCH-002 (Separation of Concerns)
/// </summary>
public class ApplicationDbContext : DbContext, IQueryDbContext
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
    /// Exposed as IQueryable through IQueryDbContext interface.
    /// </summary>
    public DbSet<Dashboard> DashboardsSet => Set<Dashboard>();

    /// <summary>
    /// DbSet for DashboardComponent entities.
    /// Exposed as IQueryable through IQueryDbContext interface.
    /// </summary>
    public DbSet<DashboardComponent> DashboardComponentsSet => Set<DashboardComponent>();

    /// <summary>
    /// IQueryDbContext implementation - Dashboards as IQueryable.
    /// </summary>
    IQueryable<Dashboard> IQueryDbContext.Dashboards => DashboardsSet;

    /// <summary>
    /// IQueryDbContext implementation - DashboardComponents as IQueryable.
    /// </summary>
    IQueryable<DashboardComponent> IQueryDbContext.DashboardComponents => DashboardComponentsSet;

    /// <summary>
    /// Materializes a query to a list asynchronously using EF Core.
    /// Compliance: ARCH-001 - EF Core call hidden behind interface
    /// </summary>
    public async Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
    {
        // Guard clause - CCP-005
        ArgumentNullException.ThrowIfNull(query);

        // This is where EF Core's ToListAsync is actually called
        // Application Layer doesn't need to know about EF Core
        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the first element or null using EF Core.
    /// Compliance: ARCH-001 - EF Core call hidden behind interface
    /// </summary>
    public async Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) where T : class
    {
        // Guard clause - CCP-005
        ArgumentNullException.ThrowIfNull(query);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Counts elements in a query using EF Core.
    /// Compliance: ARCH-001 - EF Core call hidden behind interface
    /// </summary>
    public async Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
    {
        // Guard clause - CCP-005
        ArgumentNullException.ThrowIfNull(query);

        return await query.CountAsync(cancellationToken);
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