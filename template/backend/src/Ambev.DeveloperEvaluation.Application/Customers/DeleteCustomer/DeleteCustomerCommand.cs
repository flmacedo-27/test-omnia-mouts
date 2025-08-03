using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

public class DeleteCustomerCommand : IRequest<DeleteCustomerResult>
{
    public Guid Id { get; set; }
} 