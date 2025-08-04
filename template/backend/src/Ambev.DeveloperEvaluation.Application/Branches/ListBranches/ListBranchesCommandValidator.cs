using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

/// <summary>
/// Validator for ListBranchesCommand
/// </summary>
public class ListBranchesCommandValidator : AbstractValidator<ListBranchesCommand>
{
    public ListBranchesCommandValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be between 1 and 100");
    }
} 