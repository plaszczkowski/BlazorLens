namespace BlazorLens.Application.Handlers.Queries;

/// <summary>
/// Handler for GetComponentsByDashboardIdQuery.
/// Retrieves all components for a specific dashboard with filtering and sorting.
/// Compliance: ARCH-003 (CQRS), API-004 (Filtering & Sorting)
/// </summary>
public class GetComponentsByDashboardIdQueryHandler : IRequestHandler<GetComponentsByDashboardIdQuery, OperationResult<List<ComponentDto>>>
{
    private readonly IQueryDbContext _context;

    /// <summary>
    /// Initializes a new instance of GetComponentsByDashboardIdQueryHandler.
    /// </summary>
    /// <param name="context">Database context for complex queries</param>
    /// <exception cref="ArgumentNullException">When context is null</exception>
    public GetComponentsByDashboardIdQueryHandler(IQueryDbContext context)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    /// <summary>
    /// Handles the GetComponentsByDashboardIdQuery.
    /// Compliance: API-004 (Filtering & Sorting), SEC-003 (Input Validation)
    /// </summary>
    /// <param name="request">Query request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result with list of component DTOs</returns>
    public async Task<OperationResult<List<ComponentDto>>> Handle(
        GetComponentsByDashboardIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Start with dashboard components
            var query = _context.DashboardComponents
                .Where(c => c.DashboardId == request.DashboardId);

            // Apply type filter if provided
            if (!string.IsNullOrWhiteSpace(request.TypeFilter))
            {
                query = query.Where(c => c.Type.ToString() == request.TypeFilter);
            }

            // Apply status filter if provided
            if (!string.IsNullOrWhiteSpace(request.StatusFilter))
            {
                query = query.Where(c => c.Status.ToString() == request.StatusFilter);
            }

            // Apply sorting
            query = request.SortBy switch
            {
                "Name" => request.SortDirection == "Asc"
                    ? query.OrderBy(c => c.Name)
                    : query.OrderByDescending(c => c.Name),
                "Type" => request.SortDirection == "Asc"
                    ? query.OrderBy(c => c.Type)
                    : query.OrderByDescending(c => c.Type),
                "Status" => request.SortDirection == "Asc"
                    ? query.OrderBy(c => c.Status)
                    : query.OrderByDescending(c => c.Status),
                "CreatedAt" => request.SortDirection == "Asc"
                    ? query.OrderBy(c => c.CreatedAt)
                    : query.OrderByDescending(c => c.CreatedAt),
                _ => query.OrderBy(c => c.CreatedAt) // default - preserve creation order
            };

            // Execute query with projection
            var q = query
                .Select(c => new ComponentDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type.ToString(),
                    Status = c.Status.ToString(),
                    CreatedAt = c.CreatedAt,
                    DashboardId = c.DashboardId,
                    DashboardName = c.Dashboard.Name
                });

            var components = await _context.ToListAsync(q, cancellationToken);

            return OperationResult<List<ComponentDto>>.Success(components);
        }
        catch (Exception ex)
        {
            return OperationResult<List<ComponentDto>>.Failure(
                "Failed to retrieve components.",
                ex.Message);
        }
    }
}