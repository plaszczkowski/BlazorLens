namespace BlazorLens.Infrastructure;

/// <summary>
/// Unit of Work implementation for managing database transactions.
/// Wraps DbContext transaction management with explicit control.
/// Compliance: OOD-001 (Single Responsibility), CON-005 (Fail-Fast)
/// </summary>
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _currentTransaction;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of UnitOfWork.
    /// </summary>
    /// <param name="context">Database context</param>
    /// <exception cref="ArgumentNullException">When context is null</exception>
    public UnitOfWork(ApplicationDbContext context)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    /// <summary>
    /// Indicates whether a transaction is currently active.
    /// </summary>
    public bool HasActiveTransaction => _currentTransaction != null;

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// Compliance: CON-004 (Idempotence) - can be called multiple times safely
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of state entries written to the database</returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Fail-fast if disposed - CON-005
        ObjectDisposedException.ThrowIf(_disposed, this);

        return await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Begins an explicit database transaction.
    /// Compliance: SEC-001 (Data Integrity) - explicit transaction control
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    /// <exception cref="InvalidOperationException">When transaction already active</exception>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        // Fail-fast if disposed - CON-005
        ObjectDisposedException.ThrowIf(_disposed, this);

        // Guard clause - prevent nested transactions - CCP-005
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException(
                "A transaction is already active. Commit or rollback the current transaction before starting a new one.");
        }

        _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// Commits the current transaction atomically.
    /// Compliance: ARCH-004 (Event-Driven Design) - hook for dispatching events
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    /// <exception cref="InvalidOperationException">When no active transaction</exception>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        // Fail-fast checks - CON-005
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_currentTransaction == null)
        {
            throw new InvalidOperationException(
                "No active transaction to commit. Call BeginTransactionAsync first.");
        }

        try
        {
            // Save changes within transaction
            await _context.SaveChangesAsync(cancellationToken);

            // Commit transaction
            await _currentTransaction.CommitAsync(cancellationToken);

            // TODO: Dispatch domain events here after successful commit
            // This is the hook for ARCH-004 (Event-Driven Design)
        }
        catch
        {
            // Rollback on any error
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            // Cleanup transaction
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    /// <summary>
    /// Rolls back the current transaction.
    /// All tracked changes are discarded.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    /// <exception cref="InvalidOperationException">When no active transaction</exception>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        // Fail-fast checks - CON-005
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_currentTransaction == null)
        {
            throw new InvalidOperationException(
                "No active transaction to rollback.");
        }

        try
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            // Cleanup transaction
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    /// <summary>
    /// Disposes the Unit of Work and any active transaction.
    /// Compliance: CCP-005 (Defensive Programming) - proper resource cleanup
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        // Rollback any uncommitted transaction
        if (_currentTransaction != null)
        {
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}