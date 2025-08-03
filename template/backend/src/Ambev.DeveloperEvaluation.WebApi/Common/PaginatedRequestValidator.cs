using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

/// <summary>
/// Generic validator for paginated requests.
/// </summary>
public class PaginatedRequestValidator : AbstractValidator<PaginatedRequest>
{
    /// <summary>
    /// Initializes a new instance of the PaginatedRequestValidator class.
    /// </summary>
    public PaginatedRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than zero");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be between 1 and 100");

        RuleFor(x => x.SortDirection)
            .Must(direction => string.IsNullOrEmpty(direction) || direction.ToLower() == "asc" || direction.ToLower() == "desc")
            .WithMessage("Sort direction must be 'asc' or 'desc'");
    }
}