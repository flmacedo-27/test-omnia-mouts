using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Result for cancelling a sale
/// </summary>
public class CancelSaleResult
{
    /// <summary>
    /// Gets or sets the sale ID
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the sale number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the sale status
    /// </summary>
    public SaleStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the cancellation date
    /// </summary>
    public DateTime CancelledAt { get; set; }
    
    /// <summary>
    /// Gets or sets the cancellation reason
    /// </summary>
    public string CancellationReason { get; set; } = string.Empty;
} 