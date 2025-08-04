using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale item entity
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the sale ID
    /// </summary>
    public Guid SaleId { get; set; }
    
    /// <summary>
    /// Gets or sets the sale
    /// </summary>
    public virtual Sale Sale { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the product
    /// </summary>
    public virtual Product Product { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the quantity
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Gets or sets the unit price
    /// </summary>
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// Gets or sets the discount percentage
    /// </summary>
    public decimal DiscountPercentage { get; set; }
    
    /// <summary>
    /// Gets or sets the discount amount
    /// </summary>
    public decimal DiscountAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the total amount for this item
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the item status
    /// </summary>
    public SaleItemStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the cancellation date
    /// </summary>
    public DateTime? CancelledAt { get; set; }
    
    /// <summary>
    /// Gets or sets the cancellation reason
    /// </summary>
    public string? CancellationReason { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the SaleItem class.
    /// </summary>
    public SaleItem()
    {
        Status = SaleItemStatus.Active;
    }
    
    /// <summary>
    /// Calculates the discount based on quantity
    /// </summary>
    public void CalculateDiscount()
    {
        if (Quantity < 4)
        {
            DiscountPercentage = 0;
        }
        else if (Quantity >= 4 && Quantity < 10)
        {
            DiscountPercentage = 10;
        }
        else if (Quantity >= 10 && Quantity <= 20)
        {
            DiscountPercentage = 20;
        }
        else
        {
            throw new InvalidOperationException("Cannot sell more than 20 identical items");
        }
        
        DiscountAmount = (UnitPrice * Quantity * DiscountPercentage) / 100;
        TotalAmount = (UnitPrice * Quantity) - DiscountAmount;
        UpdateTimestamp();
    }
    
    /// <summary>
    /// Cancels the item
    /// </summary>
    /// <param name="reason">The cancellation reason</param>
    public void Cancel(string reason)
    {
        Status = SaleItemStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        CancellationReason = reason;
        UpdateTimestamp();
    }
} 