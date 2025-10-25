using BlazorLens.Application.Commands;
using BlazorLens.Application.DTOs;
using BlazorLens.Application.Interfaces;
using BlazorLens.Domain.Entities;
using MediatR;

namespace BlazorLens.Application.Handlers.Commands;

/// <summary>
/// Handler for UpdateComponentCommand.
/// Updates an existing component.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public class UpdateComponentCommandHandler : IRequestHandler<UpdateComponentCommand, OperationResult<bool>>
{
    private readonly IComponentRepository _componentRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of UpdateComponentCommandHandler.
    /// </summary>
    /// <param name="componentRepository">Component repository</param>
    /// <param name="unitOfWork">Unit of work for transaction management</param>
    /// <exception cref="ArgumentNullException">When dependencies are null</exception>
    public UpdateComponentCommandHandler(
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
    /// Handles the UpdateComponentCommand.
    /// Compliance: CON-005 (Fail-Fast)
    /// </summary>
    /// <param name="request">Command request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result with success flag</returns>
    public async Task<OperationResult<bool>> Handle(
        UpdateComponentCommand request,
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

            // Update using domain methods (encapsulation)
            component.UpdateName(request.Name);
            component.UpdateDescription(request.Description);

            // Track update
            await _componentRepository.UpdateAsync(component);

            // Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return success
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            // Fail-fast error handling - CON-005
            return OperationResult<bool>.Failure(
                "Failed to update component.",
                ex.Message);
        }
    }
}