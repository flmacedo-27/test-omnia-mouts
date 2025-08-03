using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Application.Common.Paginated;

namespace Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;

public class ListCustomersResult : PaginatedResult<CustomerListItem>
{
    public IEnumerable<CustomerListItem> Customers 
    { 
        get => Items; 
        set => Items = value; 
    }
}

public class CustomerListItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public CustomerType CustomerType { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
} 