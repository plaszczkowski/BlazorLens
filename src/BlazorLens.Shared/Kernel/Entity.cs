namespace BlazorLens.Shared.Kernel;

/// <summary>
/// Base class for all entities with support for domain events.
/// Compliance: OOD-001 (Single Responsibility), ARCH-004 (Event-Driven Design)
/// </summary>
/// <typeparam name="TId">Type of entity identifier</typeparam>
public abstract class Entity<TId> where TId : notnull
{
    private readonly List<DomainEvent> _domainEvents = new();

    /// <summary>
    /// Entity unique identifier.
    /// </summary>
    public TId Id { get; protected set; }

    /// <summary>
    /// Domain events that occurred on this entity.
    /// Compliance: ARCH-004 (Event-Driven Design)
    /// </summary>
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Initializes entity with provided identifier.
    /// </summary>
    /// <param name="id">Entity identifier</param>
    protected Entity(TId id)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(id);
        Id = id;
    }

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    protected Entity()
    {
        Id = default!;
    }

    /// <summary>
    /// Adds a domain event to the entity.
    /// Events will be dispatched after successful transaction commit.
    /// </summary>
    /// <param name="domainEvent">Domain event to add</param>
    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(domainEvent);

        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears all domain events from the entity.
    /// Called by infrastructure after events are dispatched.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Determines equality based on identity.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id.Equals(other.Id);
    }

    /// <summary>
    /// Gets hash code based on entity type and identifier.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Id);
    }

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !(left == right);
    }
}