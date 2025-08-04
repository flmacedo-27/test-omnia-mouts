using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale entity
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets or sets the sale number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the sale date
    /// </summary>
    public DateTime SaleDate { get; set; }
    
    /// <summary>
    /// Gets or sets the customer ID
    /// </summary>
    public Guid CustomerId { get; set; }
    
    /// <summary>
    /// Gets or sets the customer
    /// </summary>
    public virtual Customer Customer { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the branch ID
    /// </summary>
    public Guid BranchId { get; set; }
    
    /// <summary>
    /// Gets or sets the branch
    /// </summary>
    public virtual Branch Branch { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the total sale amount
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the sale status
    /// </summary>
    public SaleStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the sale items
    /// </summary>
    public virtual ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
    
    /// <summary>
    /// Gets or sets the cancellation date
    /// </summary>
    public DateTime? CancelledAt { get; set; }
    
    /// <summary>
    /// Gets or sets the cancellation reason
    /// </summary>
    public string? CancellationReason { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        Status = SaleStatus.Active;
    }
    
    /// <summary>
    /// Calculates the total amount of the sale
    /// </summary>
    public void CalculateTotal()
    {
        TotalAmount = Items.Sum(item => item.TotalAmount);
        UpdateTimestamp();
    }
    
    /// <summary>
    /// Cancels the sale
    /// </summary>
    /// <param name="reason">The cancellation reason</param>
    public void Cancel(string reason)
    {
        Status = SaleStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        CancellationReason = reason;
        UpdateTimestamp();
    }
} 