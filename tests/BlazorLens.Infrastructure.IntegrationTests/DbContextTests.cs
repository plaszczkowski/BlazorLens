using BlazorLens.Domain.Entities;
using BlazorLens.Domain.Enums;
using BlazorLens.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BlazorLens.Infrastructure.IntegrationTests;

public class DbContextTests
{
    [Fact]
    public async Task Can_Create_And_Save_Dashboard()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_Dashboard_Create")
            .Options;

        // Act
        using (var context = new ApplicationDbContext(options))
        {
            var dashboard = new Dashboard(Guid.NewGuid(), "Test Dashboard", "Test Description");
            context.Add(dashboard);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new ApplicationDbContext(options))
        {
            var savedDashboard = await context.DashboardsSet.FirstAsync();
            Assert.NotNull(savedDashboard);
            Assert.Equal("Test Dashboard", savedDashboard.Name);
        }
    }

    [Fact]
    public async Task Can_Create_And_Save_DashboardComponent()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_Component_Create")
            .Options;

        var dashboardId = Guid.NewGuid();
        var componentId = Guid.NewGuid();

        // Act
        using (var context = new ApplicationDbContext(options))
        {
            var dashboard = new Dashboard(dashboardId, "Test Dashboard", "Test Description");
            var component = new DashboardComponent(
                componentId,
                "Test Component",
                "Test Component Description",
                ComponentType.Chart,
                dashboardId);

            context.Add(dashboard);
            context.Add(component);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new ApplicationDbContext(options))
        {
            var savedComponent = await context.DashboardComponentsSet
                .FirstAsync(c => c.Id == componentId);

            Assert.NotNull(savedComponent);
            Assert.Equal("Test Component", savedComponent.Name);
            Assert.Equal(ComponentType.Chart, savedComponent.Type);
        }
    }
}