namespace BlazorLens.Domain.Events
{
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
}
