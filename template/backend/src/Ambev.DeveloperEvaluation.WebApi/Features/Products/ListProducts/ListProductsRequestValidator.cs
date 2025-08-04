using FluentValidation;
using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

/// <summary>
/// Validator for ListProductsRequest.
/// </summary>
public class ListProductsRequestValidator : AbstractValidator<ListProductsRequest>
{
    /// <summary>
    /// Initializes a new instance of the ListProductsRequestValidator class.
    /// </summary>
    public ListProductsRequestValidator()
    {
        Include(new PaginatedRequestValidator());
    }
} 