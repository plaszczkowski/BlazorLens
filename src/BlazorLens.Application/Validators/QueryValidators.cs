namespace BlazorLens.Application.Validators;

/// <summary>
/// Validator for GetDashboardByIdQuery.
/// Compliance: SEC-003 (Input Validation), CCP-005 (Defensive Programming)
/// </summary>
public class GetDashboardByIdQueryValidator : AbstractValidator<GetDashboardByIdQuery>
{
    public GetDashboardByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Dashboard ID is required.");
    }
}

/// <summary>
/// Validator for GetAllDashboardsQuery.
/// Compliance: SEC-003 (Input Validation), CCP-005 (Defensive Programming)
/// </summary>
public class GetAllDashboardsQueryValidator : AbstractValidator<GetAllDashboardsQuery>
{
    private static readonly string[] ValidSortFields = { "Name", "CreatedAt" };
    private static readonly string[] ValidSortDirections = { "Asc", "Desc" };

    public GetAllDashboardsQueryValidator()
    {
        RuleFor(x => x.SortBy)
            .Must(field => ValidSortFields.Contains(field))
            .WithMessage($"SortBy must be one of: {string.Join(", ", ValidSortFields)}.");

        RuleFor(x => x.SortDirection)
            .Must(dir => ValidSortDirections.Contains(dir))
            .WithMessage($"SortDirection must be one of: {string.Join(", ", ValidSortDirections)}.");
    }
}

/// <summary>
/// Validator for GetDashboardsPagedQuery.
/// Compliance: SEC-003 (Input Validation), CCP-005 (Defensive Programming)
/// </summary>
public class GetDashboardsPagedQueryValidator : AbstractValidator<GetDashboardsPagedQuery>
{
    private static readonly string[] ValidSortFields = { "Name", "CreatedAt" };
    private static readonly string[] ValidSortDirections = { "Asc", "Desc" };

    public GetDashboardsPagedQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size cannot exceed 100.");

        RuleFor(x => x.SortBy)
            .Must(field => ValidSortFields.Contains(field))
            .WithMessage($"SortBy must be one of: {string.Join(", ", ValidSortFields)}.");

        RuleFor(x => x.SortDirection)
            .Must(dir => ValidSortDirections.Contains(dir))
            .WithMessage($"SortDirection must be one of: {string.Join(", ", ValidSortDirections)}.");

        RuleFor(x => x.NameFilter)
            .MaximumLength(200)
            .WithMessage("Name filter cannot exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.NameFilter));
    }
}

/// <summary>
/// Validator for GetComponentByIdQuery.
/// Compliance: SEC-003 (Input Validation), CCP-005 (Defensive Programming)
/// </summary>
public class GetComponentByIdQueryValidator : AbstractValidator<GetComponentByIdQuery>
{
    public GetComponentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Component ID is required.");
    }
}

/// <summary>
/// Validator for GetComponentsByDashboardIdQuery.
/// Compliance: SEC-003 (Input Validation), CCP-005 (Defensive Programming)
/// </summary>
public class GetComponentsByDashboardIdQueryValidator : AbstractValidator<GetComponentsByDashboardIdQuery>
{
    private static readonly string[] ValidTypes = { "Chart", "DataGrid", "Metric", "Custom" };
    private static readonly string[] ValidStatuses = { "Active", "Inactive", "Error", "Loading" };
    private static readonly string[] ValidSortFields = { "Name", "Type", "Status", "CreatedAt" };
    private static readonly string[] ValidSortDirections = { "Asc", "Desc" };

    public GetComponentsByDashboardIdQueryValidator()
    {
        RuleFor(x => x.DashboardId)
            .NotEmpty()
            .WithMessage("Dashboard ID is required.");

        RuleFor(x => x.TypeFilter)
            .Must(type => ValidTypes.Contains(type!))
            .WithMessage($"Type filter must be one of: {string.Join(", ", ValidTypes)}.")
            .When(x => !string.IsNullOrEmpty(x.TypeFilter));

        RuleFor(x => x.StatusFilter)
            .Must(status => ValidStatuses.Contains(status!))
            .WithMessage($"Status filter must be one of: {string.Join(", ", ValidStatuses)}.")
            .When(x => !string.IsNullOrEmpty(x.StatusFilter));

        RuleFor(x => x.SortBy)
            .Must(field => ValidSortFields.Contains(field))
            .WithMessage($"SortBy must be one of: {string.Join(", ", ValidSortFields)}.");

        RuleFor(x => x.SortDirection)
            .Must(dir => ValidSortDirections.Contains(dir))
            .WithMessage($"SortDirection must be one of: {string.Join(", ", ValidSortDirections)}.");
    }
}