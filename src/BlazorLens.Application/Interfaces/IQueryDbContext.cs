namespace BlazorLens.Application.Interfaces;

/// <summary>
/// Interface for query operations on the database context.
/// Provides read-only access to entities for CQRS query handlers.
/// Compliance: ARCH-001 (Clean Architecture), ARCH-003 (CQRS), OOD-002 (Dependency Inversion)
/// </summary>
public interface IQueryDbContext
{
    /// <summary>
    /// Gets queryable collection of Dashboard entities.
    /// </summary>
    IQueryable<Dashboard> Dashboards { get; }

    /// <summary>
    /// Gets queryable collection of DashboardComponent entities.
    /// </summary>
    IQueryable<DashboardComponent> DashboardComponents { get; }

    /// <summary>
    /// Materializes a query to a list asynchronously.
    /// This method abstracts away EF Core's ToListAsync from Application Layer.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="query">Query to materialize</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of entities</returns>
    Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the first element of a query or null if no element is found.
    /// This method abstracts away EF Core's FirstOrDefaultAsync from Application Layer.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="query">Query to execute</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>First element or null</returns>
    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Counts the number of elements in a query.
    /// This method abstracts away EF Core's CountAsync from Application Layer.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="query">Query to count</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Count of elements</returns>
    Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);
}