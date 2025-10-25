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