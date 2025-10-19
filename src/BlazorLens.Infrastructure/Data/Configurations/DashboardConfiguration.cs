using BlazorLens.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorLens.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Dashboard entity.
/// Compliance: ARCH-002 (Separation of Concerns), OOD-001 (Single Responsibility)
/// </summary>
public class DashboardConfiguration : IEntityTypeConfiguration<Dashboard>
{
    public void Configure(EntityTypeBuilder<Dashboard> builder)
    {
        // Guard clause - CCP-005 (Defensive Programming)
        ArgumentNullException.ThrowIfNull(builder);

        // Table configuration
        builder.ToTable("Dashboards");

        // Primary key
        builder.HasKey(d => d.Id);

        // Properties configuration
        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Dashboard display name");

        builder.Property(d => d.Description)
            .HasMaxLength(1000)
            .HasComment("Dashboard description");

        builder.Property(d => d.CreatedAt)
            .IsRequired()
            .HasComment("Dashboard creation timestamp (UTC)");

        // Relationships
        builder.HasMany(d => d.Components)
            .WithOne(c => c.Dashboard)
            .HasForeignKey(c => c.DashboardId)
            .OnDelete(DeleteBehavior.Cascade) // Cascade delete components when dashboard is deleted
            .HasConstraintName("FK_DashboardComponents_Dashboard");

        // Indexes for performance
        builder.HasIndex(d => d.Name)
            .HasDatabaseName("IX_Dashboards_Name");

        builder.HasIndex(d => d.CreatedAt)
            .HasDatabaseName("IX_Dashboards_CreatedAt");

        // Ignore domain events - they are not persisted
        // Compliance: ARCH-004 (Event-Driven Design)
        builder.Ignore(d => d.DomainEvents);
    }
}