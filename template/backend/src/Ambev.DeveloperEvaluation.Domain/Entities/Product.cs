using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product in the system.
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Gets the product name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the product code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets the product description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets the product price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets the product stock quantity.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Gets the product SKU (Stock Keeping Unit).
    /// </summary>
    public string SKU { get; set; } = string.Empty;

    /// <summary>
    /// Gets whether the product is active.
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Gets the date and time when the product was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the product.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the Product class.
    /// </summary>
    public Product()
    {
        CreatedAt = DateTime.UtcNow;
        Active = true;
    }

    /// <summary>
    /// Updates the product information.
    /// </summary>
    /// <param name="name">The new product name</param>
    /// <param name="description">The new product description</param>
    /// <param name="price">The new product price</param>
    /// <param name="stockQuantity">The new stock quantity</param>
    /// <param name="category">The new product category</param>
    /// <param name="sku">The new product SKU</param>
    public void Update(string name, string code, string description, decimal price, int stockQuantity, string sku)
    {
        Name = name;
        Code = code;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        SKU = sku;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the product stock quantity.
    /// </summary>
    /// <param name="newQuantity">The new stock quantity</param>
    public void UpdateStock(int newQuantity)
    {
        StockQuantity = newQuantity;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activates the product.
    /// Changes the product's status to Active.
    /// </summary>
    public void Activate()
    {
        Active = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the product.
    /// Changes the product's status to Inactive.
    /// </summary>
    public void Deactivate()
    {
        Active = false;
        UpdatedAt = DateTime.UtcNow;
    }
} 