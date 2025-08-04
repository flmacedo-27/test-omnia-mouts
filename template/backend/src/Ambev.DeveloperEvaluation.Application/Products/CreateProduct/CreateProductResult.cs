namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductResult
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