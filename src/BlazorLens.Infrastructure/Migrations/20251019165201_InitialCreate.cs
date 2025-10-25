using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorLens.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dashboards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Dashboard display name"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "Dashboard description"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Dashboard creation timestamp (UTC)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DashboardComponents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DashboardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Foreign key to Dashboard"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Component display name"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "Component description"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Component type (Chart, DataGrid, Metric, Custom)"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Component status (Active, Inactive, Error, Loading)"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Component creation timestamp (UTC)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DashboardComponents_Dashboard",
                        column: x => x.DashboardId,
                        principalTable: "Dashboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DashboardComponents_CreatedAt",
                table: "DashboardComponents",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardComponents_DashboardId",
                table: "DashboardComponents",
                column: "DashboardId");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardComponents_DashboardId_Status",
                table: "DashboardComponents",
                columns: new[] { "DashboardId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_DashboardComponents_Status",
                table: "DashboardComponents",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardComponents_Type",
                table: "DashboardComponents",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_CreatedAt",
                table: "Dashboards",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_Name",
                table: "Dashboards",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DashboardComponents");

            migrationBuilder.DropTable(
                name: "Dashboards");
        }
    }
}
