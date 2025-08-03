using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.ListCustomers;

/// <summary>
/// Request for listing customers.
/// </summary>
public class ListCustomersRequest : PaginatedRequest
{
    /// <summary>
    /// Gets or sets whether to include only active customers.
    /// </summary>
    public bool? ActiveOnly { get; set; } = false;

    /// <summary>
    /// Gets or sets the customer type filter.
    /// </summary>
    public CustomerType? CustomerType { get; set; }
} 