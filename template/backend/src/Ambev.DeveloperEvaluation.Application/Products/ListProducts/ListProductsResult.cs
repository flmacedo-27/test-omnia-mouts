using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

/// <summary>
/// Result for listing products with pagination.
/// </summary>
public class ListProductsResult : PaginatedResult<ProductListItem>
{
    /// <summary>
    /// Initializes a new instance of ListProductsResult
    /// </summary>
    /// <param name="products">The list of products</param>
    /// <param name="totalCount">The total count of products</param>
    /// <param name="currentPage">The current page number</param>
    /// <param name="pageSize">The page size</param>
    public ListProductsResult(IEnumerable<ProductListItem> products, int totalCount, int currentPage, int pageSize)
        : base(products, totalCount, currentPage, pageSize)
    {
    }
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
    /// Gets or sets whether the product is active.
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