namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a sale is cancelled
/// </summary>
public class SaleCancelledEvent
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
    /// Gets or sets the cancellation reason
    /// </summary>
    public string CancellationReason { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the cancellation date
    /// </summary>
    public DateTime CancelledAt { get; set; }
    
    /// <summary>
    /// Initializes a new instance of SaleCancelledEvent
    /// </summary>
    /// <param name="saleId">The sale ID</param>
    /// <param name="saleNumber">The sale number</param>
    /// <param name="cancellationReason">The cancellation reason</param>
    /// <param name="cancelledAt">The cancellation date</param>
    public SaleCancelledEvent(Guid saleId, string saleNumber, string cancellationReason, DateTime cancelledAt)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        CancellationReason = cancellationReason;
        CancelledAt = cancelledAt;
    }
} 