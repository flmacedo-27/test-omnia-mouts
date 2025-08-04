using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;

/// <summary>
/// Result for listing customers with pagination.
/// </summary>
public class ListCustomersResult : PaginatedResult<CustomerListItem>
{
    /// <summary>
    /// Initializes a new instance of ListCustomersResult
    /// </summary>
    /// <param name="customers">The list of customers</param>
    /// <param name="totalCount">The total count of customers</param>
    /// <param name="currentPage">The current page number</param>
    /// <param name="pageSize">The page size</param>
    public ListCustomersResult(IEnumerable<CustomerListItem> customers, int totalCount, int currentPage, int pageSize)
        : base(customers, totalCount, currentPage, pageSize)
    {
    }
}

/// <summary>
/// Represents a customer item in the list.
/// </summary>
public class CustomerListItem
{
    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer email.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer phone number.
    /// </summary>
    public string Phone { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer document number.
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer type.
    /// </summary>
    public CustomerType CustomerType { get; set; }
    
    /// <summary>
    /// Gets or sets whether the customer is active.
    /// </summary>
    public bool Active { get; set; }
    
    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; }
} 