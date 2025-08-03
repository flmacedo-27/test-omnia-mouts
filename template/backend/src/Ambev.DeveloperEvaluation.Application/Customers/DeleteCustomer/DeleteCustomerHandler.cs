using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, DeleteCustomerResult>
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<DeleteCustomerResult> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var success = await _customerRepository.DeleteAsync(request.Id, cancellationToken);
        
        if (!success)
        {
            return new DeleteCustomerResult
            {
                Success = false,
                Message = "Customer not found"
            };
        }

        return new DeleteCustomerResult
        {
            Success = true,
            Message = "Customer deleted successfully"
        };
    }
}