namespace BlazorLens.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for DashboardComponent entity.
/// Compliance: ARCH-002 (Separation of Concerns), OOD-001 (Single Responsibility)
/// </summary>
public class DashboardComponentConfiguration : IEntityTypeConfiguration<DashboardComponent>
{
    public void Configure(EntityTypeBuilder<DashboardComponent> builder)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(builder);

        // Table configuration
        builder.ToTable("DashboardComponents");

        // Primary key
        builder.HasKey(c => c.Id);

        // Properties from ComponentBase
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Component display name");

        builder.Property(c => c.Description)
            .HasMaxLength(1000)
            .HasComment("Component description");

        builder.Property(c => c.Type)
            .IsRequired()
            .HasConversion<string>() // Store enum as string for readability
            .HasMaxLength(50)
            .HasComment("Component type (Chart, DataGrid, Metric, Custom)");

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>() // Store enum as string
            .HasMaxLength(50)
            .HasComment("Component status (Active, Inactive, Error, Loading)");

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasComment("Component creation timestamp (UTC)");

        // DashboardComponent specific properties
        builder.Property(c => c.DashboardId)
            .IsRequired()
            .HasComment("Foreign key to Dashboard");

        // Relationships - configured on Dashboard side, but can be verified here
        builder.HasOne(c => c.Dashboard)
            .WithMany(d => d.Components)
            .HasForeignKey(c => c.DashboardId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for performance
        builder.HasIndex(c => c.DashboardId)
            .HasDatabaseName("IX_DashboardComponents_DashboardId");

        builder.HasIndex(c => c.Type)
            .HasDatabaseName("IX_DashboardComponents_Type");

        builder.HasIndex(c => c.Status)
            .HasDatabaseName("IX_DashboardComponents_Status");

        builder.HasIndex(c => c.CreatedAt)
            .HasDatabaseName("IX_DashboardComponents_CreatedAt");

        // Composite index for common query pattern
        builder.HasIndex(c => new { c.DashboardId, c.Status })
            .HasDatabaseName("IX_DashboardComponents_DashboardId_Status");

        // Ignore domain events - they are not persisted
        // Compliance: ARCH-004 (Event-Driven Design)
        builder.Ignore(c => c.DomainEvents);
    }
}