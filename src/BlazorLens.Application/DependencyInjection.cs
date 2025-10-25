using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BlazorLens.Application;

/// <summary>
/// Application layer dependency injection configuration.
/// Compliance: ARCH-001 (Clean Architecture), ARCH-003 (CQRS)
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers application services including MediatR and FluentValidation.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection for chaining</returns>
    /// <exception cref="ArgumentNullException">When services is null</exception>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(services);

        var assembly = Assembly.GetExecutingAssembly();

        // Register MediatR - automatically discovers all handlers
        // Compliance: ARCH-003 (CQRS)
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        // Register FluentValidation - automatically discovers all validators
        // Compliance: SEC-003 (Input Validation)
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}