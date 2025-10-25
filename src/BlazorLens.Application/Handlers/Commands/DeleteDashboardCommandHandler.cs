using BlazorLens.Application.Commands;
using BlazorLens.Application.DTOs;
using BlazorLens.Application.Interfaces;
using BlazorLens.Domain.Entities;
using MediatR;

namespace BlazorLens.Application.Handlers.Commands;

/// <summary>
/// Handler for DeleteDashboardCommand.
/// Deletes a dashboard and all its components (cascade).
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public class DeleteDashboardCommandHandler : IRequestHandler<DeleteDashboardCommand, OperationResult<bool>>
{
    private readonly IRepository<Dashboard> _dashboardRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of DeleteDashboardCommandHandler.
    /// </summary>
    /// <param name="dashboardRepository">Dashboard repository</param>
    /// <param name="unitOfWork">Unit of work for transaction management</param>
    /// <exception cref="ArgumentNullException">When dependencies are null</exception>
    public DeleteDashboardCommandHandler(
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
    /// Handles the DeleteDashboardCommand.
    /// Compliance: CON-005 (Fail-Fast), SEC-001 (Data Integrity - cascade delete)
    /// </summary>
    /// <param name="request">Command request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result with success flag</returns>
    public async Task<OperationResult<bool>> Handle(
        DeleteDashboardCommand request,
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

            // Delete dashboard (cascade delete components via FK)
            await _dashboardRepository.DeleteAsync(dashboard);

            // Save changes - atomic operation
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return success
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            // Fail-fast error handling - CON-005
            return OperationResult<bool>.Failure(
                "Failed to delete dashboard.",
                ex.Message);
        }
    }
}