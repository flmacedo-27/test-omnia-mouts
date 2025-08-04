using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Result for listing sales
/// </summary>
public class ListSalesResult : PaginatedResult<SaleListItem>
{
    /// <summary>
    /// Initializes a new instance of ListSalesResult
    /// </summary>
    /// <param name="items">The sale items</param>
    /// <param name="totalCount">The total count</param>
    /// <param name="currentPage">The current page</param>
    /// <param name="pageSize">The page size</param>
    public ListSalesResult(IEnumerable<SaleListItem> items, int totalCount, int currentPage, int pageSize)
        : base(items, totalCount, currentPage, pageSize)
    {
    }
}

/// <summary>
/// Sale list item
/// </summary>
public class SaleListItem
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
    /// Gets or sets the sale date
    /// </summary>
    public DateTime SaleDate { get; set; }
    
    /// <summary>
    /// Gets or sets the customer name
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the branch name
    /// </summary>
    public string BranchName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the total amount
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the sale status
    /// </summary>
    public SaleStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the number of items
    /// </summary>
    public int ItemsCount { get; set; }
} 