namespace BlazorLens.Application.Interfaces;

/// <summary>
/// Unit of Work pattern for managing database transactions.
/// Provides explicit transaction control for complex operations.
/// Compliance: OOD-001 (Single Responsibility), ARCH-001 (Clean Architecture)
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Saves all changes made in this context to the database.
    /// Use for simple operations without explicit transactions.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of state entries written to the database</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins an explicit database transaction.
    /// Use for complex operations requiring multiple steps with rollback capability.
    /// Compliance: SEC-001 (Data Integrity)
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction.
    /// All changes are persisted to the database atomically.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    /// <exception cref="InvalidOperationException">When no active transaction exists</exception>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction.
    /// All changes are discarded.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    /// <exception cref="InvalidOperationException">When no active transaction exists</exception>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Indicates whether a transaction is currently active.
    /// </summary>
    bool HasActiveTransaction { get; }
}