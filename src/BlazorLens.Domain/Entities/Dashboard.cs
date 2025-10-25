namespace BlazorLens.Domain.Entities;

public class Dashboard : Entity<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    
    // Nawigacja - kolekcja komponentów w dashboardzie
    public virtual ICollection<DashboardComponent> Components { get; private set; } = new List<DashboardComponent>();

    public Dashboard(Guid id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }

    // Metody domenowe
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be empty", nameof(newName));
            
        Name = newName;
    }

    public void UpdateDescription(string newDescription)
    {
        Description = newDescription ?? string.Empty;
    }

    public void AddComponent(DashboardComponent component)
    {
        if (component == null)
            throw new ArgumentNullException(nameof(component));
            
        Components.Add(component);
    }

    // Konstruktor bezparametrowy dla EF Core
    protected Dashboard() { }
}