using BlazorLens.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorLens.Domain.Entities
{
    public class DashboardComponent : ComponentBase
    {
        public Guid DashboardId { get; private set; }

        public DashboardComponent(Guid id, string name, string description, ComponentType type, Guid dashboardId)
            : base(id, name, description, type)
        {
            DashboardId = dashboardId;
        }

        protected DashboardComponent() { }
    }
}
