using BlazorLens.Domain.Enums;
using BlazorLens.Shared.Kernel;

namespace BlazorLens.Domain.Entities;

/// <summary>
/// Base class for all component types.
/// Provides common properties and domain methods.
/// Compliance: OOD-001 (Single Responsibility), OOD-005 (Design by Contract)
/// </summary>
public abstract class ComponentBase : Entity<Guid>
{
    public string Name { get; protected set; } = string.Empty;
    public string Description { get; protected set; } = string.Empty;
    public ComponentType Type { get; protected set; }
    public ComponentStatus Status { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    /// <summary>
    /// Initializes a new component.
    /// </summary>
    /// <param name="id">Component identifier</param>
    /// <param name="name">Component name</param>
    /// <param name="description">Component description</param>
    /// <param name="type">Component type</param>
    /// <exception cref="ArgumentException">When name is empty</exception>
    protected ComponentBase(Guid id, string name, string description, ComponentType type)
        : base(id)
    {
        // Guard clauses - CCP-005 (Defensive Programming)
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Component name cannot be empty.", nameof(name));

        Name = name;
        Description = description ?? string.Empty;
        Type = type;
        Status = ComponentStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    protected ComponentBase() { }

    /// <summary>
    /// Updates the component name.
    /// Domain method ensuring business rules.
    /// Compliance: OOD-005 (Design by Contract)
    /// </summary>
    /// <param name="newName">New component name</param>
    /// <exception cref="ArgumentException">When name is empty or too long</exception>
    public void UpdateName(string newName)
    {
        // Guard clauses - CCP-005 (Defensive Programming)
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Component name cannot be empty.", nameof(newName));

        if (newName.Length > 200)
            throw new ArgumentException("Component name cannot exceed 200 characters.", nameof(newName));

        if (newName.Length < 3)
            throw new ArgumentException("Component name must be at least 3 characters.", nameof(newName));

        Name = newName;
    }

    /// <summary>
    /// Updates the component description.
    /// Domain method ensuring business rules.
    /// Compliance: OOD-005 (Design by Contract)
    /// </summary>
    /// <param name="newDescription">New component description</param>
    /// <exception cref="ArgumentException">When description is too long</exception>
    public void UpdateDescription(string newDescription)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        if (newDescription != null && newDescription.Length > 1000)
            throw new ArgumentException("Component description cannot exceed 1000 characters.", nameof(newDescription));

        Description = newDescription ?? string.Empty;
    }

    /// <summary>
    /// Changes the component status.
    /// Domain method with event raising capability.
    /// Compliance: ARCH-004 (Event-Driven Design)
    /// </summary>
    /// <param name="newStatus">New component status</param>
    /// <param name="reason">Optional reason for status change</param>
    public void ChangeStatus(ComponentStatus newStatus, string? reason = null)
    {
        // Guard clause - CCP-005
        if (newStatus == Status)
            return; // No change needed

        var oldStatus = Status;
        Status = newStatus;

        // TODO: Raise domain event - ComponentStatusChangedEvent
        // This will be implemented when we wire domain event dispatching
        // AddDomainEvent(new ComponentStatusChangedEvent
        // {
        //     ComponentId = Id,
        //     OldStatus = oldStatus.ToString(),
        //     NewStatus = newStatus.ToString(),
        //     Reason = reason
        // });
    }

    /// <summary>
    /// Activates the component.
    /// Convenience method for common operation.
    /// </summary>
    public void Activate()
    {
        ChangeStatus(ComponentStatus.Active, "Component activated");
    }

    /// <summary>
    /// Deactivates the component.
    /// Convenience method for common operation.
    /// </summary>
    public void Deactivate()
    {
        ChangeStatus(ComponentStatus.Inactive, "Component deactivated");
    }

    /// <summary>
    /// Marks the component as errored.
    /// Convenience method for error handling.
    /// </summary>
    /// <param name="errorReason">Reason for error</param>
    public void MarkAsError(string errorReason)
    {
        ChangeStatus(ComponentStatus.Error, errorReason);
    }
}