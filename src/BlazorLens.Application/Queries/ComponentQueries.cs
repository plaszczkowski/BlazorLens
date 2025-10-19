using BlazorLens.Application.DTOs;
using MediatR;

namespace BlazorLens.Application.Queries;

/// <summary>
/// Query to get a Component by its identifier.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public sealed record GetComponentByIdQuery : IRequest<OperationResult<ComponentDto>>
{
    /// <summary>
    /// Component identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Creates a new GetComponentByIdQuery.
    /// </summary>
    /// <param name="id">Component identifier</param>
    public GetComponentByIdQuery(Guid id)
    {
        Id = id;
    }
}

/// <summary>
/// Query to get all Components for a specific Dashboard.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public sealed record GetComponentsByDashboardIdQuery : IRequest<OperationResult<List<ComponentDto>>>
{
    /// <summary>
    /// Dashboard identifier.
    /// </summary>
    public Guid DashboardId { get; init; }

    /// <summary>
    /// Optional: Filter by component type.
    /// Valid values: "Chart", "DataGrid", "Metric", "Custom"
    /// </summary>
    public string? TypeFilter { get; init; }

    /// <summary>
    /// Optional: Filter by component status.
    /// Valid values: "Active", "Inactive", "Error", "Loading"
    /// </summary>
    public string? StatusFilter { get; init; }

    /// <summary>
    /// Optional: Sort by field name.
    /// Valid values: "Name", "Type", "Status", "CreatedAt"
    /// Default: "CreatedAt"
    /// </summary>
    public string SortBy { get; init; } = "CreatedAt";

    /// <summary>
    /// Optional: Sort direction.
    /// Valid values: "Asc", "Desc"
    /// Default: "Asc" (oldest first - preserve creation order)
    /// </summary>
    public string SortDirection { get; init; } = "Asc";

    /// <summary>
    /// Creates a new GetComponentsByDashboardIdQuery.
    /// </summary>
    /// <param name="dashboardId">Dashboard identifier</param>
    public GetComponentsByDashboardIdQuery(Guid dashboardId)
    {
        DashboardId = dashboardId;
    }
}

/// <summary>
/// Query to get all Components (all dashboards).
/// Compliance: ARCH-003 (CQRS), API-004 (Pagination)
/// </summary>
public sealed record GetAllComponentsQuery : IRequest<OperationResult<List<ComponentDto>>>
{
    /// <summary>
    /// Optional: Filter by component type.
    /// </summary>
    public string? TypeFilter { get; init; }

    /// <summary>
    /// Optional: Filter by component status.
    /// </summary>
    public string? StatusFilter { get; init; }

    /// <summary>
    /// Optional: Include dashboard name in results.
    /// Default: true
    /// </summary>
    public bool IncludeDashboardName { get; init; } = true;
}