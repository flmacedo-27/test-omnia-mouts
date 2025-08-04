using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Command for listing sales with pagination
/// </summary>
public class ListSalesCommand : IRequest<ListSalesResult>
{
    /// <summary>
    /// Gets or sets the page number
    /// </summary>
    public int PageNumber { get; set; } = 1;
    
    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int PageSize { get; set; } = 10;
} 