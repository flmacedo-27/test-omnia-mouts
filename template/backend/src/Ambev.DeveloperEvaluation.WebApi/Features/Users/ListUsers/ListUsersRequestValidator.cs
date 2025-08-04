using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;

/// <summary>
/// Validator for ListUsersRequest that defines input validation rules.
/// </summary>
public class ListUsersRequestValidator : AbstractValidator<ListUsersRequest>
{
    /// <summary>
    /// Initializes a new instance of the ListUsersRequestValidator with input validation rules.
    /// </summary>
    /// <remarks>
    /// Input validation rules include:
    /// - PageNumber: Must be greater than 0
    /// - PageSize: Must be between 1 and 100
    /// </remarks>
    public ListUsersRequestValidator()
    {
        RuleFor(request => request.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(request => request.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100");
    }
} 