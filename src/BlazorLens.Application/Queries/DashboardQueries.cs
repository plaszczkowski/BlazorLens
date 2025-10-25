namespace BlazorLens.Application.Queries;

/// <summary>
/// Query to get a Dashboard by its identifier.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public sealed record GetDashboardByIdQuery : IRequest<OperationResult<DashboardDto>>
{
    /// <summary>
    /// Dashboard identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Creates a new GetDashboardByIdQuery.
    /// </summary>
    /// <param name="id">Dashboard identifier</param>
    public GetDashboardByIdQuery(Guid id)
    {
        Id = id;
    }
}

/// <summary>
/// Query to get all Dashboards.
/// Compliance: ARCH-003 (CQRS), API-004 (Pagination)
/// </summary>
public sealed record GetAllDashboardsQuery : IRequest<OperationResult<List<DashboardDto>>>
{
    /// <summary>
    /// Optional: Include component count in results.
    /// Default: true
    /// </summary>
    public bool IncludeComponentCount { get; init; } = true;

    /// <summary>
    /// Optional: Sort by field name.
    /// Valid values: "Name", "CreatedAt"
    /// Default: "CreatedAt"
    /// </summary>
    public string SortBy { get; init; } = "CreatedAt";

    /// <summary>
    /// Optional: Sort direction.
    /// Valid values: "Asc", "Desc"
    /// Default: "Desc" (newest first)
    /// </summary>
    public string SortDirection { get; init; } = "Desc";
}

/// <summary>
/// Query to get Dashboards with pagination support.
/// Compliance: ARCH-003 (CQRS), API-004 (Pagination & Filtering)
/// </summary>
public sealed record GetDashboardsPagedQuery : IRequest<OperationResult<PagedResult<DashboardDto>>>
{
    /// <summary>
    /// Page number (1-based).
    /// </summary>
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; init; } = 10;

    /// <summary>
    /// Optional: Filter by name (contains).
    /// </summary>
    public string? NameFilter { get; init; }

    /// <summary>
    /// Optional: Sort by field name.
    /// Valid values: "Name", "CreatedAt"
    /// Default: "CreatedAt"
    /// </summary>
    public string SortBy { get; init; } = "CreatedAt";

    /// <summary>
    /// Optional: Sort direction.
    /// Valid values: "Asc", "Desc"
    /// Default: "Desc" (newest first)
    /// </summary>
    public string SortDirection { get; init; } = "Desc";
}