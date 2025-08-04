using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.ListCustomers;

/// <summary>
/// Response for listing customers with pagination.
/// </summary>
public class ListCustomersResponse : PaginatedList<CustomerListItem>
{
    /// <summary>
    /// Initializes a new instance of ListCustomersResponse
    /// </summary>
    /// <param name="customers">The list of customers</param>
    /// <param name="totalCount">The total count of customers</param>
    /// <param name="currentPage">The current page number</param>
    /// <param name="pageSize">The page size</param>
    public ListCustomersResponse(IEnumerable<CustomerListItem> customers, int totalCount, int currentPage, int pageSize)
        : base(customers.ToList(), totalCount, currentPage, pageSize)
    {
    }
} 