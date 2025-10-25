using BlazorLens.Application.DTOs;
using BlazorLens.Application.Interfaces;
using BlazorLens.Application.Queries;
using MediatR;

namespace BlazorLens.Application.Handlers.Queries;

/// <summary>
/// Handler for GetAllComponentsQuery.
/// Retrieves all components across all dashboards with filtering.
/// Compliance: ARCH-003 (CQRS), API-004 (Filtering)
/// </summary>
public class GetAllComponentsQueryHandler : IRequestHandler<GetAllComponentsQuery, OperationResult<List<ComponentDto>>>
{
    private readonly IQueryDbContext _context;

    /// <summary>
    /// Initializes a new instance of GetAllComponentsQueryHandler.
    /// </summary>
    /// <param name="context">Database context for complex queries</param>
    /// <exception cref="ArgumentNullException">When context is null</exception>
    public GetAllComponentsQueryHandler(IQueryDbContext context)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    /// <summary>
    /// Handles the GetAllComponentsQuery.
    /// Compliance: API-004 (Filtering), SEC-003 (Input Validation)
    /// </summary>
    /// <param name="request">Query request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result with list of component DTOs</returns>
    public async Task<OperationResult<List<ComponentDto>>> Handle(
        GetAllComponentsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Start with all components
            var query = _context.DashboardComponents.AsQueryable();

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

            // Order by creation date (default)
            query = query.OrderBy(c => c.CreatedAt);

            // Execute query with projection
            var components = await query
                .Select(c => new ComponentDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type.ToString(),
                    Status = c.Status.ToString(),
                    CreatedAt = c.CreatedAt,
                    DashboardId = c.DashboardId,
                    DashboardName = request.IncludeDashboardName
                        ? c.Dashboard.Name
                        : string.Empty
                })
                .ToListAsync(cancellationToken);

            return OperationResult<List<ComponentDto>>.Success(components);
        }
        catch (Exception ex)
        {
            return OperationResult<List<ComponentDto>>.Failure(
                "Failed to retrieve all components.",
                ex.Message);
        }
    }
}