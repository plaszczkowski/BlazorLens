using BlazorLens.Application.Interfaces;
using BlazorLens.Infrastructure.Data;
using BlazorLens.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorLens.Infrastructure;

/// <summary>
/// Infrastructure layer dependency injection configuration.
/// Compliance: ARCH-001 (Clean Architecture), ARCH-002 (Separation of Concerns)
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers infrastructure services including database context and repositories.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Application configuration</param>
    /// <returns>Service collection for chaining</returns>
    /// <exception cref="ArgumentNullException">When services or configuration is null</exception>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Guard clauses - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        // Get connection string - SEC-004 (Secrets Management)
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        // Register DbContext with SQL Server
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                connectionString,
                sqlOptions =>
                {
                    // Enable retry on failure - CLOUD-002 (Circuit Breaker pattern)
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null);

                    // Command timeout
                    sqlOptions.CommandTimeout(30);

                    // Migration assembly
                    sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                });

            // Enable sensitive data logging in development only
            // SEC-001 (Least Privilege) - don't log sensitive data in production
#if DEBUG
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
#endif
        });

        // Register repositories - OOD-001 (Single Responsibility Principle)
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IComponentRepository, ComponentRepository>();

        // Register Unit of Work - HYBRID APPROACH
        // Simple operations: inject ApplicationDbContext directly
        // Complex operations: inject IUnitOfWork for explicit transaction control
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register IQueryDbContext for query handlers - ARCH-001 (Clean Architecture)
        // Application Layer depends on interface, not concrete implementation
        services.AddScoped<IQueryDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}