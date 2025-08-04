namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

/// <summary>
/// Request for creating a new product.
/// </summary>
public class CreateProductRequest
{
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
} 