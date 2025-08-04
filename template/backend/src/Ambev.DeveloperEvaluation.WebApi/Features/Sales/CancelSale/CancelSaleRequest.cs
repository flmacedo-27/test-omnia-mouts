namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// Request for cancelling a sale
/// </summary>
public class CancelSaleRequest
{
    /// <summary>
    /// Gets or sets the cancellation reason
    /// </summary>
    public string Reason { get; set; } = string.Empty;
} 