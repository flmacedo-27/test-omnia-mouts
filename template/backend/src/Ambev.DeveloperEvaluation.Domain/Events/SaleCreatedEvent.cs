using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a sale is created
/// </summary>
public class SaleCreatedEvent : INotification
{
    /// <summary>
    /// Gets or sets the sale ID
    /// </summary>
    public Guid SaleId { get; set; }
    
    /// <summary>
    /// Gets or sets the sale number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer ID
    /// </summary>
    public Guid CustomerId { get; set; }
    
    /// <summary>
    /// Gets or sets the branch ID
    /// </summary>
    public Guid BranchId { get; set; }
    
    /// <summary>
    /// Gets or sets the total amount
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Initializes a new instance of SaleCreatedEvent
    /// </summary>
    /// <param name="saleId">The sale ID</param>
    /// <param name="saleNumber">The sale number</param>
    /// <param name="customerId">The customer ID</param>
    /// <param name="branchId">The branch ID</param>
    /// <param name="totalAmount">The total amount</param>
    /// <param name="createdAt">The creation date</param>
    public SaleCreatedEvent(Guid saleId, string saleNumber, Guid customerId, Guid branchId, decimal totalAmount, DateTime createdAt)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        CustomerId = customerId;
        BranchId = branchId;
        TotalAmount = totalAmount;
        CreatedAt = createdAt;
    }
} 