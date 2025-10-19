using BlazorLens.Application.DTOs;
using MediatR;

namespace BlazorLens.Application.Commands;

/// <summary>
/// Command to create a new Dashboard.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public sealed record CreateDashboardCommand : IRequest<OperationResult<Guid>>
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
/// Command to update an existing Dashboard.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public sealed record UpdateDashboardCommand : IRequest<OperationResult<bool>>
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

/// <summary>
/// Command to delete a Dashboard.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public sealed record DeleteDashboardCommand : IRequest<OperationResult<bool>>
{
    /// <summary>
    /// Dashboard identifier to delete.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Creates a new DeleteDashboardCommand.
    /// </summary>
    /// <param name="id">Dashboard identifier</param>
    public DeleteDashboardCommand(Guid id)
    {
        Id = id;
    }
}