using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;

/// <summary>
/// Validator for ListCustomersCommand
/// </summary>
public class ListCustomersCommandValidator : AbstractValidator<ListCustomersCommand>
{
    public ListCustomersCommandValidator()
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