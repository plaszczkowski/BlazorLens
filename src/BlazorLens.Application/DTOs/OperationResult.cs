namespace BlazorLens.Application.DTOs;

/// <summary>
/// Generic response wrapper for operations.
/// Provides consistent response structure across all commands/queries.
/// Compliance: API-001 (Contract-First), CON-005 (Fail-Fast)
/// </summary>
/// <typeparam name="T">Type of data in response</typeparam>
public sealed record OperationResult<T>
{
    /// <summary>
    /// Indicates whether the operation succeeded.
    /// </summary>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// Result data if operation succeeded.
    /// Null if operation failed.
    /// </summary>
    public T? Data { get; init; }

    /// <summary>
    /// Error message if operation failed.
    /// Null if operation succeeded.
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Detailed error information (for debugging).
    /// Should not be exposed to end users in production.
    /// </summary>
    public string? ErrorDetails { get; init; }

    /// <summary>
    /// Collection of validation errors.
    /// Used when input validation fails.
    /// Compliance: SEC-003 (Input Validation)
    /// </summary>
    public Dictionary<string, string[]>? ValidationErrors { get; init; }

    /// <summary>
    /// Creates a successful operation result.
    /// </summary>
    /// <param name="data">Result data</param>
    /// <returns>Success result</returns>
    public static OperationResult<T> Success(T data)
    {
        return new OperationResult<T>
        {
            IsSuccess = true,
            Data = data
        };
    }

    /// <summary>
    /// Creates a failed operation result.
    /// </summary>
    /// <param name="errorMessage">Error message</param>
    /// <param name="errorDetails">Detailed error information</param>
    /// <returns>Failure result</returns>
    public static OperationResult<T> Failure(string errorMessage, string? errorDetails = null)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            ErrorDetails = errorDetails
        };
    }

    /// <summary>
    /// Creates a validation failure result.
    /// </summary>
    /// <param name="validationErrors">Validation errors dictionary</param>
    /// <returns>Validation failure result</returns>
    public static OperationResult<T> ValidationFailure(Dictionary<string, string[]> validationErrors)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            ErrorMessage = "Validation failed",
            ValidationErrors = validationErrors
        };
    }
}

/// <summary>
/// Paginated result wrapper for list queries.
/// Compliance: API-004 (Pagination & Filtering)
/// </summary>
/// <typeparam name="T">Type of items in list</typeparam>
public sealed record PagedResult<T>
{
    /// <summary>
    /// List of items for current page.
    /// </summary>
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();

    /// <summary>
    /// Current page number (1-based).
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Total number of items across all pages.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Indicates whether there is a previous page.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Indicates whether there is a next page.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;
}