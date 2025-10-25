using BlazorLens.Application.Commands;
using BlazorLens.Application.DTOs;
using BlazorLens.Application.Interfaces;
using BlazorLens.Domain.Entities;
using MediatR;

namespace BlazorLens.Application.Handlers.Commands;

/// <summary>
/// Handler for UpdateDashboardCommand.
/// Updates an existing dashboard.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public class UpdateDashboardCommandHandler : IRequestHandler<UpdateDashboardCommand, OperationResult<bool>>
{
    private readonly IRepository<Dashboard> _dashboardRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of UpdateDashboardCommandHandler.
    /// </summary>
    /// <param name="dashboardRepository">Dashboard repository</param>
    /// <param name="unitOfWork">Unit of work for transaction management</param>
    /// <exception cref="ArgumentNullException">When dependencies are null</exception>
    public UpdateDashboardCommandHandler(
        IRepository<Dashboard> dashboardRepository,
        IUnitOfWork unitOfWork)
    {
        // Guard clauses - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(dashboardRepository);
        ArgumentNullException.ThrowIfNull(unitOfWork);

        _dashboardRepository = dashboardRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the UpdateDashboardCommand.
    /// Compliance: CON-005 (Fail-Fast)
    /// </summary>
    /// <param name="request">Command request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result with success flag</returns>
    public async Task<OperationResult<bool>> Handle(
        UpdateDashboardCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Find existing dashboard
            var dashboard = await _dashboardRepository.GetByIdAsync(request.Id);

            // Fail-fast if not found - CON-005
            if (dashboard == null)
            {
                return OperationResult<bool>.Failure(
                    $"Dashboard with ID {request.Id} not found.");
            }

            // Update using domain methods (encapsulation)
            dashboard.UpdateName(request.Name);
            dashboard.UpdateDescription(request.Description);

            // Track update
            await _dashboardRepository.UpdateAsync(dashboard);

            // Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return success
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            // Fail-fast error handling - CON-005
            return OperationResult<bool>.Failure(
                "Failed to update dashboard.",
                ex.Message);
        }
    }
}