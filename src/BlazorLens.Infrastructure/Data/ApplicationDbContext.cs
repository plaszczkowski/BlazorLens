using BlazorLens.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorLens.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // DbSet dla naszych encji
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<DashboardComponent> DashboardComponents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tutaj później dodamy konfigurację encji
            // modelBuilder.ApplyConfiguration(new DashboardConfiguration());
        }
    }
}
