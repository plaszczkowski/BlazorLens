namespace BlazorLens.Domain.Entities;

public class DashboardComponent : ComponentBase
{
    public Guid DashboardId { get; private set; }

    // Nawigacja do Dashboard
    public virtual Dashboard Dashboard { get; private set; } = null!;

    public DashboardComponent(Guid id, string name, string description, ComponentType type, Guid dashboardId)
        : base(id, name, description, type)
    {
        DashboardId = dashboardId;
    }

    // Metody domenowe
    public void ChangeDashboard(Guid newDashboardId)
    {
        DashboardId = newDashboardId;
    }

    protected DashboardComponent() { }
}