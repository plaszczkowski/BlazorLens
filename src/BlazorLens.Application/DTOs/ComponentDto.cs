namespace BlazorLens.Application.DTOs;

/// <summary>
/// Data Transfer Object for DashboardComponent entity.
/// Used for queries and responses.
/// Compliance: ARCH-003 (CQRS), ARCH-002 (Separation of Concerns)
/// </summary>
public sealed record ComponentDto
{
    /// <summary>
    /// Component unique identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Component display name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Component description.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Component type (Chart, DataGrid, Metric, Custom).
    /// </summary>
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Component status (Active, Inactive, Error, Loading).
    /// </summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Creation timestamp (UTC).
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Dashboard identifier this component belongs to.
    /// </summary>
    public Guid DashboardId { get; init; }

    /// <summary>
    /// Dashboard name (for display purposes).
    /// </summary>
    public string DashboardName { get; init; } = string.Empty;
}

/// <summary>
/// Data Transfer Object for creating a new Component.
/// Used in commands.
/// Compliance: SEC-003 (Input Validation)
/// </summary>
public sealed record CreateComponentDto
{
    /// <summary>
    /// Component display name.
    /// Required, max 200 characters.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Component description.
    /// Optional, max 1000 characters.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Component type (Chart, DataGrid, Metric, Custom).
    /// Required.
    /// </summary>
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Dashboard identifier to add component to.
    /// Required.
    /// </summary>
    public Guid DashboardId { get; init; }
}

/// <summary>
/// Data Transfer Object for updating an existing Component.
/// Used in commands.
/// Compliance: SEC-003 (Input Validation)
/// </summary>
public sealed record UpdateComponentDto
{
    /// <summary>
    /// Component identifier to update.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// New component name.
    /// Required, max 200 characters.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// New component description.
    /// Optional, max 1000 characters.
    /// </summary>
    public string Description { get; init; } = string.Empty;
}