namespace BlazorLens.Application.Interfaces
{
    public interface IComponentRepository : IRepository<DashboardComponent>
    {
        Task<List<DashboardComponent>> GetByDashboardIdAsync(Guid dashboardId);
    }
}
