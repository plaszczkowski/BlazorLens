using BlazorLens.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BlazorLens.Application.Interfaces;

/// <summary>
/// Interface for query operations on the database context.
/// Provides read-only access to DbSets for CQRS query handlers.
/// Compliance: ARCH-001 (Clean Architecture), ARCH-003 (CQRS), OOD-002 (Dependency Inversion)
/// </summary>
public interface IQueryDbContext
{
    /// <summary>
    /// Gets the DbSet for Dashboard entities.
    /// </summary>
    DbSet<Dashboard> Dashboards { get; }

    /// <summary>
    /// Gets the DbSet for DashboardComponent entities.
    /// </summary>
    DbSet<DashboardComponent> DashboardComponents { get; }

    /// <summary>
    /// Executes a raw SQL query and returns results.
    /// Use for complex queries that cannot be expressed in LINQ.
    /// Compliance: SEC-003 (Input Validation) - use parameters, not string concatenation
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="sql">SQL query</param>
    /// <param name="parameters">Query parameters</param>
    /// <returns>Queryable result set</returns>
    IQueryable<T> SqlQuery<T>(string sql, params object[] parameters) where T : class;
}