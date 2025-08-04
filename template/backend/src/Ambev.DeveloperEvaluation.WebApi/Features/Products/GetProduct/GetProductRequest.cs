using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

/// <summary>
/// Request for getting a product.
/// </summary>
public class GetProductRequest
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public Guid Id { get; set; }
} 