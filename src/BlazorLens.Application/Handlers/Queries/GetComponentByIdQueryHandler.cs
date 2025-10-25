using BlazorLens.Application.DTOs;
using BlazorLens.Application.Interfaces;
using BlazorLens.Application.Queries;
using BlazorLens.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlazorLens.Application.Handlers.Queries;

/// <summary>
/// Handler for GetComponentByIdQuery.
/// Retrieves a single component by ID with dashboard name.
/// Compliance: ARCH-003 (CQRS), OOD-001 (Single Responsibility)
/// </summary>
public class GetComponentByIdQueryHandler : IRequestHandler<GetComponentByIdQuery, OperationResult<ComponentDto>>
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of GetComponentByIdQueryHandler.
    /// </summary>
    /// <param name="context">Database context for joins</param>
    /// <exception cref="ArgumentNullException">When context is null</exception>
    public GetComponentByIdQueryHandler(ApplicationDbContext context)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    /// <summary>
    /// Handles the GetComponentByIdQuery.
    /// Compliance: CON-005 (Fail-Fast)
    /// </summary>
    /// <param name="request">Query request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result with component DTO</returns>
    public async Task<OperationResult<ComponentDto>> Handle(
        GetComponentByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Query with dashboard join for name
            var dto = await _context.DashboardComponents
                .Where(c => c.Id == request.Id)
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
                })
                .FirstOrDefaultAsync(cancellationToken);

            // Fail-fast if not found - CON-005
            if (dto == null)
            {
                return OperationResult<ComponentDto>.Failure(
                    $"Component with ID {request.Id} not found.");
            }

            return OperationResult<ComponentDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return OperationResult<ComponentDto>.Failure(
                "Failed to retrieve component.",
                ex.Message);
        }
    }
}