using BlazorLens.Application.Interfaces;
using BlazorLens.Domain.Entities;
using BlazorLens.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorLens.Infrastructure.Repositories;

/// <summary>
/// Repository for DashboardComponent entities with dashboard-specific queries.
/// Compliance: OOD-001 (Single Responsibility), ARCH-002 (Separation of Concerns)
/// </summary>
public class ComponentRepository : Repository<DashboardComponent>, IComponentRepository
{
    /// <summary>
    /// Initializes a new instance of ComponentRepository.
    /// </summary>
    /// <param name="context">Database context</param>
    /// <exception cref="ArgumentNullException">When context is null</exception>
    public ComponentRepository(ApplicationDbContext context) : base(context)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(context);
    }

    /// <summary>
    /// Gets all components for a specific dashboard.
    /// </summary>
    /// <param name="dashboardId">Dashboard identifier</param>
    /// <returns>List of components belonging to the dashboard</returns>
    /// <exception cref="ArgumentException">When dashboardId is empty</exception>
    public virtual async Task<List<DashboardComponent>> GetByDashboardIdAsync(Guid dashboardId)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        if (dashboardId == Guid.Empty)
        {
            throw new ArgumentException("Dashboard ID cannot be empty.", nameof(dashboardId));
        }

        // Query with explicit filtering - SEC-003 (Input Validation)
        return await _entities
            .Where(c => c.DashboardId == dashboardId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }
}