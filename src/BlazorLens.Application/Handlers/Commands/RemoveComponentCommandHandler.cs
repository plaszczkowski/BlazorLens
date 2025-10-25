using BlazorLens.Application.Commands;
using BlazorLens.Application.DTOs;
using BlazorLens.Application.Interfaces;
using BlazorLens.Domain.Entities;
using MediatR;

namespace BlazorLens.Application.Handlers.Commands;

/// <summary>
/// Handler for RemoveComponentCommand.
/// Removes a component from a dashboard.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public class RemoveComponentCommandHandler : IRequestHandler<RemoveComponentCommand, OperationResult<bool>>
{
    private readonly IComponentRepository _componentRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of RemoveComponentCommandHandler.
    /// </summary>
    /// <param name="componentRepository">Component repository</param>
    /// <param name="unitOfWork">Unit of work for transaction management</param>
    /// <exception cref="ArgumentNullException">When dependencies are null</exception>
    public RemoveComponentCommandHandler(
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
    /// Handles the RemoveComponentCommand.
    /// Compliance: CON-005 (Fail-Fast)
    /// </summary>
    /// <param name="request">Command request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result with success flag</returns>
    public async Task<OperationResult<bool>> Handle(
        RemoveComponentCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Find existing component
            var component = await _componentRepository.GetByIdAsync(request.Id);

            // Fail-fast if not found - CON-005
            if (component == null)
            {
                return OperationResult<bool>.Failure(
                    $"Component with ID {request.Id} not found.");
            }

            // Delete component
            await _componentRepository.DeleteAsync(component);

            // Save changes - atomic operation
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return success
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            // Fail-fast error handling - CON-005
            return OperationResult<bool>.Failure(
                "Failed to remove component.",
                ex.Message);
        }
    }
}