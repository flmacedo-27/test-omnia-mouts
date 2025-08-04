using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

/// <summary>
/// Response for listing products.
/// </summary>
public class ListProductsResponse : PaginatedResponseBase<ProductListItem>
{
}

/// <summary>
/// Represents a product item in the list.
/// </summary>
public class ProductListItem
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the product code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the product stock quantity.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Gets or sets the product SKU.
    /// </summary>
    public string SKU { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product active.
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last update date.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}