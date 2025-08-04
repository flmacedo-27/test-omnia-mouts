using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

/// <summary>
/// Request for listing products.
/// </summary>
public class ListProductsRequest : PaginatedRequest
{
    /// <summary>
    /// Gets or sets whether to include only active products.
    /// </summary>
    public bool ActiveOnly { get; set; }
} 