# Unit of Work - Usage Examples

## Hybrid Approach Philosophy

This project uses a **hybrid approach** for transaction management:
- **Simple operations**: Use `DbContext.SaveChangesAsync()` directly
- **Complex operations**: Use `IUnitOfWork` for explicit transaction control

---

## Pattern 1: Simple Operation (Direct DbContext)

Use when you have a single, straightforward operation.

```csharp
public class CreateDashboardHandler
{
    private readonly IRepository<Dashboard> _dashboardRepo;
    private readonly ApplicationDbContext _context;

    public CreateDashboardHandler(
        IRepository<Dashboard> dashboardRepo,
        ApplicationDbContext context)
    {
        _dashboardRepo = dashboardRepo;
        _context = context;
    }

    public async Task<Guid> Handle(CreateDashboardCommand command)
    {
        // Create entity
        var dashboard = new Dashboard(
            Guid.NewGuid(),
            command.Name,
            command.Description);

        // Track entity (no save yet)
        await _dashboardRepo.AddAsync(dashboard);

        // Simple save - DbContext is already a Unit of Work
        await _context.SaveChangesAsync();

        return dashboard.Id;
    }
}
```

**When to use:**
- ✅ Single entity creation/update
- ✅ No need for explicit rollback logic
- ✅ Simple business rules

---

## Pattern 2: Complex Operation (Explicit UnitOfWork)

Use when you need explicit transaction control with multiple steps.

```csharp
public class CreateDashboardWithComponentsHandler
{
    private readonly IRepository<Dashboard> _dashboardRepo;
    private readonly IComponentRepository _componentRepo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDashboardWithComponentsHandler(
        IRepository<Dashboard> dashboardRepo,
        IComponentRepository componentRepo,
        IUnitOfWork unitOfWork)
    {
        _dashboardRepo = dashboardRepo;
        _componentRepo = componentRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateDashboardWithComponentsCommand command)
    {
        // Begin explicit transaction
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Step 1: Create dashboard
            var dashboard = new Dashboard(
                Guid.NewGuid(),
                command.Name,
                command.Description);
            
            await _dashboardRepo.AddAsync(dashboard);

            // Step 2: Create multiple components
            foreach (var componentDto in command.Components)
            {
                var component = new DashboardComponent(
                    Guid.NewGuid(),
                    componentDto.Name,
                    componentDto.Description,
                    componentDto.Type,
                    dashboard.Id);
                
                await _componentRepo.AddAsync(component);
            }

            // Step 3: External service call (example)
            // await _notificationService.NotifyAsync(...);

            // Commit ALL changes atomically
            await _unitOfWork.CommitTransactionAsync();

            return dashboard.Id;
        }
        catch (Exception ex)
        {
            // Rollback on any error
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
```

**When to use:**
- ✅ Multiple related entities
- ✅ Need for explicit rollback
- ✅ External service calls within transaction
- ✅ Complex business rules with multiple validation steps

---

## Pattern 3: Nested Operations (Advanced)

When calling multiple handlers that each use repositories.

```csharp
public class BulkCreateDashboardsHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateDashboardHandler _createHandler;

    public async Task Handle(BulkCreateCommand command)
    {
        // Single transaction for all dashboards
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            foreach (var dashboardDto in command.Dashboards)
            {
                // Call nested handler (uses repositories only, no save)
                await _createHandler.Handle(dashboardDto);
            }

            // Single commit for ALL operations
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
```

---

## Anti-Patterns (DON'T DO THIS!)

### ❌ Anti-Pattern 1: Mixing approaches

```csharp
// ❌ BAD - mixing UoW and direct saves
await _unitOfWork.BeginTransactionAsync();
await _dashboardRepo.AddAsync(dashboard);
await _context.SaveChangesAsync(); // ❌ Don't save inside UoW transaction!
await _unitOfWork.CommitTransactionAsync();
```

### ❌ Anti-Pattern 2: Multiple SaveChanges without transaction

```csharp
// ❌ BAD - multiple saves without transaction
await _dashboardRepo.AddAsync(dashboard);
await _context.SaveChangesAsync(); // commit #1

await _componentRepo.AddAsync(component);
await _context.SaveChangesAsync(); // commit #2
// What if #2 fails? #1 is already in database!
```

### ❌ Anti-Pattern 3: Forgetting to dispose

```csharp
// ❌ BAD - not disposing UnitOfWork
var uow = new UnitOfWork(context);
await uow.BeginTransactionAsync();
// ... operations ...
// ❌ Forgot to commit/rollback AND dispose!
```

**Correct approach:**
```csharp
// ✅ GOOD - using dependency injection (auto-disposed)
public class Handler
{
    private readonly IUnitOfWork _unitOfWork; // injected, auto-disposed by DI
}
```

---

## Decision Tree

```
Is this a simple, single-entity operation?
├─ YES → Use DbContext.SaveChangesAsync() directly
└─ NO → Continue...

Do you need to rollback on errors?
├─ YES → Use IUnitOfWork
└─ NO → Continue...

Are there multiple related entities?
├─ YES → Use IUnitOfWork
└─ NO → Use DbContext.SaveChangesAsync()

Are there external service calls?
├─ YES → Use IUnitOfWork
└─ NO → Use DbContext.SaveChangesAsync()
```

---

## Best Practices

1. **Keep it simple**: Default to `DbContext.SaveChangesAsync()` for simple operations
2. **Explicit is better**: Use `IUnitOfWork` when transaction boundaries are complex
3. **Fail fast**: Always have try/catch with rollback for explicit transactions
4. **One transaction per use case**: Don't nest transactions unnecessarily
5. **Idempotency**: Design operations to be safely retryable

---

## Compliance

- ✅ **OOD-001** (Single Responsibility): Repository tracks, UoW manages transactions
- ✅ **ARCH-001** (Clean Architecture): Application Layer controls transaction boundaries
- ✅ **SEC-001** (Data Integrity): Atomic operations prevent partial updates
- ✅ **CON-004** (Idempotence): Operations can be safely retried
- ✅ **CON-005** (Fail-Fast): Explicit error handling with rollback