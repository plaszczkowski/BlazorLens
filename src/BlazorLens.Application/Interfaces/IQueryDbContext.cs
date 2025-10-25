using BlazorLens.Domain.Entities;

namespace BlazorLens.Application.Interfaces;

/// <summary>
/// Interface for query operations on the database context.
/// Provides read-only access to entities for CQRS query handlers.
/// Compliance: ARCH-001 (Clean Architecture), ARCH-003 (CQRS), OOD-002 (Dependency Inversion)
/// </summary>
public interface IQueryDbContext
{
    /// <summary>
    /// Gets queryable collection of Dashboard entities.
    /// </summary>
    IQueryable<Dashboard> Dashboards { get; }

    /// <summary>
    /// Gets queryable collection of DashboardComponent entities.
    /// </summary>
    IQueryable<DashboardComponent> DashboardComponents { get; }
}