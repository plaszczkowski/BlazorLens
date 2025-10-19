namespace BlazorLens.Application.DTOs;

/// <summary>
/// Data Transfer Object for Dashboard entity.
/// Used for queries and responses.
/// Compliance: ARCH-003 (CQRS), ARCH-002 (Separation of Concerns)
/// </summary>
public sealed record DashboardDto
{
    /// <summary>
    /// Dashboard unique identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Dashboard display name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Dashboard description.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Creation timestamp (UTC).
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Number of components in this dashboard.
    /// </summary>
    public int ComponentCount { get; init; }
}

/// <summary>
/// Data Transfer Object for creating a new Dashboard.
/// Used in commands.
/// Compliance: SEC-003 (Input Validation)
/// </summary>
public sealed record CreateDashboardDto
{
    /// <summary>
    /// Dashboard display name.
    /// Required, max 200 characters.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Dashboard description.
    /// Optional, max 1000 characters.
    /// </summary>
    public string Description { get; init; } = string.Empty;
}

/// <summary>
/// Data Transfer Object for updating an existing Dashboard.
/// Used in commands.
/// Compliance: SEC-003 (Input Validation)
/// </summary>
public sealed record UpdateDashboardDto
{
    /// <summary>
    /// Dashboard identifier to update.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// New dashboard name.
    /// Required, max 200 characters.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// New dashboard description.
    /// Optional, max 1000 characters.
    /// </summary>
    public string Description { get; init; } = string.Empty;
}