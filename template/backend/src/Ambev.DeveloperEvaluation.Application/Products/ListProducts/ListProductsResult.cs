using Ambev.DeveloperEvaluation.Application.Common.Paginated;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsResult : PaginatedResult<ProductListItem>
{
    public IEnumerable<ProductListItem> Products 
    { 
        get => Items; 
        set => Items = value; 
    }
}

public class ProductListItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string SKU { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
} 