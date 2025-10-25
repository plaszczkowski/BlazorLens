using BlazorLens.Application.DTOs;
using BlazorLens.Application.Interfaces;
using BlazorLens.Application.Queries;
using MediatR;

namespace BlazorLens.Application.Handlers.Queries;

/// <summary>
/// Handler for GetDashboardsPagedQuery.
/// Retrieves dashboards with pagination, filtering, and sorting.
/// Compliance: ARCH-003 (CQRS), API-004 (Pagination & Filtering)
/// </summary>
public class GetDashboardsPagedQueryHandler : IRequestHandler<GetDashboardsPagedQuery, OperationResult<PagedResult<DashboardDto>>>
{
    private readonly IQueryDbContext _context;

    /// <summary>
    /// Initializes a new instance of GetDashboardsPagedQueryHandler.
    /// </summary>
    /// <param name="context">Database context for complex queries</param>
    /// <exception cref="ArgumentNullException">When context is null</exception>
    public GetDashboardsPagedQueryHandler(IQueryDbContext context)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    /// <summary>
    /// Handles the GetDashboardsPagedQuery.
    /// Compliance: API-004 (Pagination & Filtering), SEC-003 (Input Validation)
    /// </summary>
    /// <param name="request">Query request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result with paged dashboard DTOs</returns>
    public async Task<OperationResult<PagedResult<DashboardDto>>> Handle(
        GetDashboardsPagedQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Start with queryable
            var query = _context.Dashboards.AsQueryable();

            // Apply name filter if provided
            if (!string.IsNullOrWhiteSpace(request.NameFilter))
            {
                query = query.Where(d => d.Name.Contains(request.NameFilter));
            }

            // Get total count for pagination
            var totalCount = await query.CountAsync(cancellationToken);

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

            // Apply pagination
            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(d => new DashboardDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    CreatedAt = d.CreatedAt,
                    ComponentCount = d.Components.Count
                })
                .ToListAsync(cancellationToken);

            // Create paged result
            var pagedResult = new PagedResult<DashboardDto>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return OperationResult<PagedResult<DashboardDto>>.Success(pagedResult);
        }
        catch (Exception ex)
        {
            return OperationResult<PagedResult<DashboardDto>>.Failure(
                "Failed to retrieve paged dashboards.",
                ex.Message);
        }
    }
}