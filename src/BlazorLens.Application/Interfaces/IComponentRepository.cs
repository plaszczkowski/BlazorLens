using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorLens.Domain.Entities;

namespace BlazorLens.Application.Interfaces
{
    public interface IComponentRepository : IRepository<DashboardComponent>
    {
        Task<List<DashboardComponent>> GetByDashboardIdAsync(Guid dashboardId);
    }
}
