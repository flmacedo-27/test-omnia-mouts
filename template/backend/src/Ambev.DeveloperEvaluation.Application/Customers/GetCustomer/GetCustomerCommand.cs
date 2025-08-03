using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

public class GetCustomerCommand : IRequest<GetCustomerResult?>
{
    public Guid Id { get; set; }
} 