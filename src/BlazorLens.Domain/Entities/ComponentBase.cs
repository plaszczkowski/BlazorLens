using BlazorLens.Domain.Enums;
using BlazorLens.Shared.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorLens.Domain.Entities
{
    public abstract class ComponentBase : Entity<Guid>
    {
        public string Name { get; protected set; } = string.Empty;
        public string Description { get; protected set; } = string.Empty;
        public ComponentType Type { get; protected set; }
        public ComponentStatus Status { get; protected set; }
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

        protected ComponentBase(Guid id, string name, string description, ComponentType type)
            : base(id)
        {
            Name = name;
            Description = description;
            Type = type;
            Status = ComponentStatus.Active;
        }

        protected ComponentBase() { }
    }
}
