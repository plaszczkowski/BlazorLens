namespace BlazorLens.Domain.Events
{
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
}
