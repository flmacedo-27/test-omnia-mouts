using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.ListCustomers;

/// <summary>
/// Validator for ListCustomersRequest.
/// </summary>
public class ListCustomersRequestValidator : AbstractValidator<ListCustomersRequest>
{
    /// <summary>
    /// Initializes a new instance of the ListCustomersRequestValidator class.
    /// </summary>
    public ListCustomersRequestValidator()
    {
        Include(new PaginatedRequestValidator());
    }
} 