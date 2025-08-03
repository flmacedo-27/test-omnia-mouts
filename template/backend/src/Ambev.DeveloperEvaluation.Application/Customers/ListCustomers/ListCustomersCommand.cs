using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;

public class ListCustomersCommand : IRequest<ListCustomersResult>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 