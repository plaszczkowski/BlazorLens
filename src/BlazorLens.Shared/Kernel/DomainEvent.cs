namespace BlazorLens.Shared.Kernel;

/// <summary>
/// Base class for all domain events.
/// Domain events represent something that happened in the domain that domain experts care about.
/// Compliance: ARCH-004 (Event-Driven Design), OOD-001 (Single Responsibility)
/// </summary>
public abstract record DomainEvent
{
    /// <summary>
    /// Unique identifier of the event.
    /// </summary>
    public Guid EventId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// When the event occurred (UTC).
    /// </summary>
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Optional correlation identifier for tracing related events.
    /// Compliance: OBS-003 (Distributed Tracing)
    /// </summary>
    public string? CorrelationId { get; init; }
}