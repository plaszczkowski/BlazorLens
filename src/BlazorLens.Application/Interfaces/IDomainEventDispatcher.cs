using BlazorLens.Shared.Kernel;

namespace BlazorLens.Application.Interfaces;

/// <summary>
/// Interface for dispatching domain events to handlers.
/// This is a hook for future implementation using MediatR.
/// Compliance: ARCH-004 (Event-Driven Design), OOD-002 (Interface Segregation)
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches a single domain event to all registered handlers.
    /// </summary>
    /// <param name="domainEvent">Event to dispatch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task DispatchAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dispatches multiple domain events to all registered handlers.
    /// Events are dispatched in order.
    /// </summary>
    /// <param name="domainEvents">Events to dispatch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task DispatchAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken = default);
}