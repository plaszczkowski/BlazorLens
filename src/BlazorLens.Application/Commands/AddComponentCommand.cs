using BlazorLens.Application.DTOs;
using MediatR;

namespace BlazorLens.Application.Commands;

/// <summary>
/// Command to add a new Component to a Dashboard.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public sealed record AddComponentCommand : IRequest<OperationResult<Guid>>
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
/// Command to update an existing Component.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public sealed record UpdateComponentCommand : IRequest<OperationResult<bool>>
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

/// <summary>
/// Command to remove a Component from a Dashboard.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public sealed record RemoveComponentCommand : IRequest<OperationResult<bool>>
{
    /// <summary>
    /// Component identifier to remove.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Creates a new RemoveComponentCommand.
    /// </summary>
    /// <param name="id">Component identifier</param>
    public RemoveComponentCommand(Guid id)
    {
        Id = id;
    }
}

/// <summary>
/// Command to change a Component's status.
/// Compliance: ARCH-003 (CQRS), ARCH-004 (Event-Driven)
/// </summary>
public sealed record ChangeComponentStatusCommand : IRequest<OperationResult<bool>>
{
    /// <summary>
    /// Component identifier.
    /// </summary>
    public Guid ComponentId { get; init; }

    /// <summary>
    /// New status (Active, Inactive, Error, Loading).
    /// </summary>
    public string NewStatus { get; init; } = string.Empty;

    /// <summary>
    /// Optional reason for status change.
    /// </summary>
    public string? Reason { get; init; }
}