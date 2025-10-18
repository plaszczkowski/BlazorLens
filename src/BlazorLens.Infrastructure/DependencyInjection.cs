using BlazorLens.Application.Interfaces;
using BlazorLens.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorLens.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Rejestracja DbContext (tymczasowo - bez bazy)
            services.AddDbContext<DbContext>(options =>
            {
                // Tymczasowo używamy InMemory - później zmienimy na SQL Server
                options.UseInMemoryDatabase("BlazorLensDb");
            });

            // Rejestracja repozytoriów
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}
