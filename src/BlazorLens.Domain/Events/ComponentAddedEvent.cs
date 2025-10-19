using BlazorLens.Shared.Kernel;

namespace BlazorLens.Domain.Events;

/// <summary>
/// Event raised when a new component is added to a dashboard.
/// Compliance: ARCH-004 (Event-Driven Design)
/// </summary>
public sealed record ComponentAddedEvent : DomainEvent
{
    /// <summary>
    /// Dashboard identifier where component was added.
    /// </summary>
    public Guid DashboardId { get; init; }

    /// <summary>
    /// Component identifier that was added.
    /// </summary>
    public Guid ComponentId { get; init; }

    /// <summary>
    /// Component name.
    /// </summary>
    public string ComponentName { get; init; } = string.Empty;

    /// <summary>
    /// Component type.
    /// </summary>
    public string ComponentType { get; init; } = string.Empty;
}

/// <summary>
/// Event raised when a component is removed from a dashboard.
/// Compliance: ARCH-004 (Event-Driven Design)
/// </summary>
public sealed record ComponentRemovedEvent : DomainEvent
{
    /// <summary>
    /// Dashboard identifier where component was removed from.
    /// </summary>
    public Guid DashboardId { get; init; }

    /// <summary>
    /// Component identifier that was removed.
    /// </summary>
    public Guid ComponentId { get; init; }

    /// <summary>
    /// Component name.
    /// </summary>
    public string ComponentName { get; init; } = string.Empty;
}

/// <summary>
/// Event raised when a component's status changes.
/// Compliance: ARCH-004 (Event-Driven Design)
/// </summary>
public sealed record ComponentStatusChangedEvent : DomainEvent
{
    /// <summary>
    /// Component identifier.
    /// </summary>
    public Guid ComponentId { get; init; }

    /// <summary>
    /// Previous status.
    /// </summary>
    public string OldStatus { get; init; } = string.Empty;

    /// <summary>
    /// New status.
    /// </summary>
    public string NewStatus { get; init; } = string.Empty;

    /// <summary>
    /// Reason for status change (optional).
    /// </summary>
    public string? Reason { get; init; }
}