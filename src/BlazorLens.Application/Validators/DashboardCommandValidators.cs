namespace BlazorLens.Application.Validators;

/// <summary>
/// Validator for CreateDashboardCommand.
/// Compliance: SEC-003 (Input Validation), CCP-005 (Defensive Programming)
/// </summary>
public class CreateDashboardCommandValidator : AbstractValidator<CreateDashboardCommand>
{
    public CreateDashboardCommandValidator()
    {
        // Name validation - required, length constraints
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Dashboard name is required.")
            .MaximumLength(200)
            .WithMessage("Dashboard name cannot exceed 200 characters.")
            .MinimumLength(3)
            .WithMessage("Dashboard name must be at least 3 characters.");

        // Description validation - optional, max length
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Dashboard description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

/// <summary>
/// Validator for UpdateDashboardCommand.
/// Compliance: SEC-003 (Input Validation), CCP-005 (Defensive Programming)
/// </summary>
public class UpdateDashboardCommandValidator : AbstractValidator<UpdateDashboardCommand>
{
    public UpdateDashboardCommandValidator()
    {
        // Id validation - must not be empty
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Dashboard ID is required.");

        // Name validation - required, length constraints
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Dashboard name is required.")
            .MaximumLength(200)
            .WithMessage("Dashboard name cannot exceed 200 characters.")
            .MinimumLength(3)
            .WithMessage("Dashboard name must be at least 3 characters.");

        // Description validation - optional, max length
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Dashboard description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

/// <summary>
/// Validator for DeleteDashboardCommand.
/// Compliance: SEC-003 (Input Validation), CCP-005 (Defensive Programming)
/// </summary>
public class DeleteDashboardCommandValidator : AbstractValidator<DeleteDashboardCommand>
{
    public DeleteDashboardCommandValidator()
    {
        // Id validation - must not be empty
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Dashboard ID is required.");
    }
}