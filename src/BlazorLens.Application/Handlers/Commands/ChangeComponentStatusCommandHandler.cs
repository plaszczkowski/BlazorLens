using BlazorLens.Application.Commands;
using BlazorLens.Application.DTOs;
using BlazorLens.Application.Interfaces;
using BlazorLens.Domain.Entities;
using BlazorLens.Domain.Enums;
using MediatR;

namespace BlazorLens.Application.Handlers.Commands;

/// <summary>
/// Handler for ChangeComponentStatusCommand.
/// Changes a component's status with optional reason.
/// Compliance: ARCH-003 (CQRS), ARCH-004 (Event-Driven), OOD-001 (Single Responsibility)
/// </summary>
public class ChangeComponentStatusCommandHandler : IRequestHandler<ChangeComponentStatusCommand, OperationResult<bool>>
{
    private readonly IComponentRepository _componentRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of ChangeComponentStatusCommandHandler.
    /// </summary>
    /// <param name="componentRepository">Component repository</param>
    /// <param name="unitOfWork">Unit of work for transaction management</param>
    /// <exception cref="ArgumentNullException">When dependencies are null</exception>
    public ChangeComponentStatusCommandHandler(
        IComponentRepository componentRepository,
        IUnitOfWork unitOfWork)
    {
        // Guard clauses - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(componentRepository);
        ArgumentNullException.ThrowIfNull(unitOfWork);

        _componentRepository = componentRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the ChangeComponentStatusCommand.
    /// Compliance: CON-005 (Fail-Fast), ARCH-004 (Event-Driven Design)
    /// </summary>
    /// <param name="request">Command request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result with success flag</returns>
    public async Task<OperationResult<bool>> Handle(
        ChangeComponentStatusCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Find existing component
            var component = await _componentRepository.GetByIdAsync(request.ComponentId);

            // Fail-fast if not found - CON-005
            if (component == null)
            {
                return OperationResult<bool>.Failure(
                    $"Component with ID {request.ComponentId} not found.");
            }

            // Parse enum - validation already done by FluentValidation
            if (!Enum.TryParse<ComponentStatus>(request.NewStatus, out var newStatus))
            {
                return OperationResult<bool>.Failure(
                    $"Invalid component status: {request.NewStatus}");
            }

            // Use domain method to change status
            // This will trigger domain event in the future (see ComponentBase.ChangeStatus TODO)
            // Compliance: ARCH-004 (Event-Driven Design)
            component.ChangeStatus(newStatus, request.Reason);

            // Track update
            await _componentRepository.UpdateAsync(component);

            // Save changes - will dispatch domain events in future
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return success
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            // Fail-fast error handling - CON-005
            return OperationResult<bool>.Failure(
                "Failed to change component status.",
                ex.Message);
        }
    }
}