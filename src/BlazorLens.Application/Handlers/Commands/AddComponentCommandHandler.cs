using BlazorLens.Application.Commands;
using BlazorLens.Application.DTOs;
using BlazorLens.Application.Interfaces;
using BlazorLens.Domain.Entities;
using BlazorLens.Domain.Enums;
using MediatR;

namespace BlazorLens.Application.Handlers.Commands;

/// <summary>
/// Handler for AddComponentCommand.
/// Adds a new component to a dashboard.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public class AddComponentCommandHandler : IRequestHandler<AddComponentCommand, OperationResult<Guid>>
{
    private readonly IRepository<Dashboard> _dashboardRepository;
    private readonly IComponentRepository _componentRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of AddComponentCommandHandler.
    /// </summary>
    /// <param name="dashboardRepository">Dashboard repository</param>
    /// <param name="componentRepository">Component repository</param>
    /// <param name="unitOfWork">Unit of work for transaction management</param>
    /// <exception cref="ArgumentNullException">When dependencies are null</exception>
    public AddComponentCommandHandler(
        IRepository<Dashboard> dashboardRepository,
        IComponentRepository componentRepository,
        IUnitOfWork unitOfWork)
    {
        // Guard clauses - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(dashboardRepository);
        ArgumentNullException.ThrowIfNull(componentRepository);
        ArgumentNullException.ThrowIfNull(unitOfWork);

        _dashboardRepository = dashboardRepository;
        _componentRepository = componentRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the AddComponentCommand.
    /// Compliance: CON-005 (Fail-Fast), SEC-001 (Data Integrity)
    /// </summary>
    /// <param name="request">Command request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result with new component ID</returns>
    public async Task<OperationResult<Guid>> Handle(
        AddComponentCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Verify dashboard exists - fail-fast - CON-005
            var dashboard = await _dashboardRepository.GetByIdAsync(request.DashboardId);
            if (dashboard == null)
            {
                return OperationResult<Guid>.Failure(
                    $"Dashboard with ID {request.DashboardId} not found.");
            }

            // Parse enum - validation already done by FluentValidation
            if (!Enum.TryParse<ComponentType>(request.Type, out var componentType))
            {
                return OperationResult<Guid>.Failure(
                    $"Invalid component type: {request.Type}");
            }

            // Create domain entity
            var component = new DashboardComponent(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                componentType,
                request.DashboardId);

            // Track entity
            await _componentRepository.AddAsync(component);

            // Save changes - simple operation, no explicit transaction
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return success with new ID
            return OperationResult<Guid>.Success(component.Id);
        }
        catch (Exception ex)
        {
            // Fail-fast error handling - CON-005
            return OperationResult<Guid>.Failure(
                "Failed to add component.",
                ex.Message);
        }
    }
}