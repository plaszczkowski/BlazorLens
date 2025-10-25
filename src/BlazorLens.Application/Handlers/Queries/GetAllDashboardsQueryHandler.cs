using BlazorLens.Application.DTOs;
using BlazorLens.Application.Interfaces;
using BlazorLens.Application.Queries;
using MediatR;

namespace BlazorLens.Application.Handlers.Queries;

/// <summary>
/// Handler for GetAllDashboardsQuery.
/// Retrieves all dashboards with sorting options.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public class GetAllDashboardsQueryHandler : IRequestHandler<GetAllDashboardsQuery, OperationResult<List<DashboardDto>>>
{
    private readonly IQueryDbContext _context;

    /// <summary>
    /// Initializes a new instance of GetAllDashboardsQueryHandler.
    /// </summary>
    /// <param name="context">Database context for complex queries</param>
    /// <exception cref="ArgumentNullException">When context is null</exception>
    public GetAllDashboardsQueryHandler(IQueryDbContext context)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    /// <summary>
    /// Handles the GetAllDashboardsQuery.
    /// Compliance: API-004 (Sorting)
    /// </summary>
    /// <param name="request">Query request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result with list of dashboard DTOs</returns>
    public async Task<OperationResult<List<DashboardDto>>> Handle(
        GetAllDashboardsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Start with queryable
            var query = _context.Dashboards.AsQueryable();

            // Apply sorting
            query = request.SortBy switch
            {
                "Name" => request.SortDirection == "Asc"
                    ? query.OrderBy(d => d.Name)
                    : query.OrderByDescending(d => d.Name),
                "CreatedAt" => request.SortDirection == "Asc"
                    ? query.OrderBy(d => d.CreatedAt)
                    : query.OrderByDescending(d => d.CreatedAt),
                _ => query.OrderByDescending(d => d.CreatedAt) // default
            };

            // Execute query with projection
            var q = query
                .Select(d => new DashboardDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    CreatedAt = d.CreatedAt,
                    ComponentCount = request.IncludeComponentCount
                        ? d.Components.Count
                        : 0
                });

            var components = await _context.ToListAsync(q, cancellationToken);

            return OperationResult<List<DashboardDto>>.Success(components);
        }
        catch (Exception ex)
        {
            return OperationResult<List<DashboardDto>>.Failure(
                "Failed to retrieve dashboards.",
                ex.Message);
        }
    }
}