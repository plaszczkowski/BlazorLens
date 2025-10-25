using BlazorLens.Application.Commands;
using BlazorLens.Application.DTOs;
using BlazorLens.Application.Interfaces;
using BlazorLens.Domain.Entities;
using MediatR;

namespace BlazorLens.Application.Handlers.Commands;

/// <summary>
/// Handler for CreateDashboardCommand.
/// Creates a new dashboard in the system.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public class CreateDashboardCommandHandler : IRequestHandler<CreateDashboardCommand, OperationResult<Guid>>
{
    private readonly IRepository<Dashboard> _dashboardRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of CreateDashboardCommandHandler.
    /// </summary>
    /// <param name="dashboardRepository">Dashboard repository</param>
    /// <param name="unitOfWork">Unit of work for transaction management</param>
    /// <exception cref="ArgumentNullException">When dependencies are null</exception>
    public CreateDashboardCommandHandler(
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
    /// Handles the CreateDashboardCommand.
    /// Compliance: CON-005 (Fail-Fast), SEC-001 (Data Integrity)
    /// </summary>
    /// <param name="request">Command request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result with new dashboard ID</returns>
    public async Task<OperationResult<Guid>> Handle(
        CreateDashboardCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Create domain entity
            var dashboard = new Dashboard(
                Guid.NewGuid(),
                request.Name,
                request.Description);

            // Track entity (no save yet)
            await _dashboardRepository.AddAsync(dashboard);

            // Simple operation - use UnitOfWork.SaveChangesAsync (not explicit transaction)
            // Compliance: Hybrid UnitOfWork approach
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return success with new ID
            return OperationResult<Guid>.Success(dashboard.Id);
        }
        catch (Exception ex)
        {
            // Fail-fast error handling - CON-005
            return OperationResult<Guid>.Failure(
                "Failed to create dashboard.",
                ex.Message);
        }
    }
}