namespace BlazorLens.Application.Handlers.Queries;

/// <summary>
/// Handler for GetDashboardByIdQuery.
/// Retrieves a single dashboard by ID with component count.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public class GetDashboardByIdQueryHandler : IRequestHandler<GetDashboardByIdQuery, OperationResult<DashboardDto>>
{
    private readonly IRepository<Dashboard> _dashboardRepository;

    /// <summary>
    /// Initializes a new instance of GetDashboardByIdQueryHandler.
    /// </summary>
    /// <param name="dashboardRepository">Dashboard repository</param>
    /// <exception cref="ArgumentNullException">When repository is null</exception>
    public GetDashboardByIdQueryHandler(IRepository<Dashboard> dashboardRepository)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(dashboardRepository);
        _dashboardRepository = dashboardRepository;
    }

    /// <summary>
    /// Handles the GetDashboardByIdQuery.
    /// Compliance: CON-005 (Fail-Fast)
    /// </summary>
    /// <param name="request">Query request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result with dashboard DTO</returns>
    public async Task<OperationResult<DashboardDto>> Handle(
        GetDashboardByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Find dashboard
            var dashboard = await _dashboardRepository.GetByIdAsync(request.Id);

            // Fail-fast if not found - CON-005
            if (dashboard == null)
            {
                return OperationResult<DashboardDto>.Failure(
                    $"Dashboard with ID {request.Id} not found.");
            }

            // Map to DTO
            var dto = new DashboardDto
            {
                Id = dashboard.Id,
                Name = dashboard.Name,
                Description = dashboard.Description,
                CreatedAt = dashboard.CreatedAt,
                ComponentCount = dashboard.Components?.Count ?? 0
            };

            return OperationResult<DashboardDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return OperationResult<DashboardDto>.Failure(
                "Failed to retrieve dashboard.",
                ex.Message);
        }
    }
}