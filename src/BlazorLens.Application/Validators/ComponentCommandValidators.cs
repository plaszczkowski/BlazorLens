namespace BlazorLens.Application.Validators;

/// <summary>
/// Validator for AddComponentCommand.
/// Compliance: SEC-003 (Input Validation), CCP-005 (Defensive Programming)
/// </summary>
public class AddComponentCommandValidator : AbstractValidator<AddComponentCommand>
{
    // Valid component types from ComponentType enum
    private static readonly string[] ValidTypes = { "Chart", "DataGrid", "Metric", "Custom" };

    public AddComponentCommandValidator()
    {
        // Name validation - required, length constraints
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Component name is required.")
            .MaximumLength(200)
            .WithMessage("Component name cannot exceed 200 characters.")
            .MinimumLength(3)
            .WithMessage("Component name must be at least 3 characters.");

        // Description validation - optional, max length
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Component description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        // Type validation - required, must be valid enum value
        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Component type is required.")
            .Must(type => ValidTypes.Contains(type))
            .WithMessage($"Component type must be one of: {string.Join(", ", ValidTypes)}.");

        // DashboardId validation - must not be empty
        RuleFor(x => x.DashboardId)
            .NotEmpty()
            .WithMessage("Dashboard ID is required.");
    }
}

/// <summary>
/// Validator for UpdateComponentCommand.
/// Compliance: SEC-003 (Input Validation), CCP-005 (Defensive Programming)
/// </summary>
public class UpdateComponentCommandValidator : AbstractValidator<UpdateComponentCommand>
{
    public UpdateComponentCommandValidator()
    {
        // Id validation - must not be empty
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Component ID is required.");

        // Name validation - required, length constraints
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Component name is required.")
            .MaximumLength(200)
            .WithMessage("Component name cannot exceed 200 characters.")
            .MinimumLength(3)
            .WithMessage("Component name must be at least 3 characters.");

        // Description validation - optional, max length
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Component description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

/// <summary>
/// Validator for RemoveComponentCommand.
/// Compliance: SEC-003 (Input Validation), CCP-005 (Defensive Programming)
/// </summary>
public class RemoveComponentCommandValidator : AbstractValidator<RemoveComponentCommand>
{
    public RemoveComponentCommandValidator()
    {
        // Id validation - must not be empty
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Component ID is required.");
    }
}

/// <summary>
/// Validator for ChangeComponentStatusCommand.
/// Compliance: SEC-003 (Input Validation), CCP-005 (Defensive Programming)
/// </summary>
public class ChangeComponentStatusCommandValidator : AbstractValidator<ChangeComponentStatusCommand>
{
    // Valid component statuses from ComponentStatus enum
    private static readonly string[] ValidStatuses = { "Active", "Inactive", "Error", "Loading" };

    public ChangeComponentStatusCommandValidator()
    {
        // ComponentId validation - must not be empty
        RuleFor(x => x.ComponentId)
            .NotEmpty()
            .WithMessage("Component ID is required.");

        // NewStatus validation - required, must be valid enum value
        RuleFor(x => x.NewStatus)
            .NotEmpty()
            .WithMessage("New status is required.")
            .Must(status => ValidStatuses.Contains(status))
            .WithMessage($"Status must be one of: {string.Join(", ", ValidStatuses)}.");

        // Reason validation - optional, max length
        RuleFor(x => x.Reason)
            .MaximumLength(500)
            .WithMessage("Reason cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Reason));
    }
}