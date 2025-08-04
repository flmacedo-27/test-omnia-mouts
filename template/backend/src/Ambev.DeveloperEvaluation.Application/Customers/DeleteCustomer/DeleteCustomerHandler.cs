using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

public class DeleteCustomerHandler : BaseHandler<DeleteCustomerCommand, DeleteCustomerResult, DeleteCustomerCommandValidator>
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerHandler(
        ICustomerRepository customerRepository,
        IMapper mapper,
        ILogger<DeleteCustomerHandler> logger,
        DeleteCustomerCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _customerRepository = customerRepository;
    }

    protected override async Task<DeleteCustomerResult> ExecuteAsync(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        await _customerRepository.DeleteAsync(request.Id, cancellationToken);

        return new DeleteCustomerResult
        {
            Success = true,
            Message = "Customer deleted successfully"
        };
    }

    protected override void LogOperationStart(DeleteCustomerCommand request)
    {
        Logger.LogInformation("Deleting customer with ID: {CustomerId}", request.Id);
    }

    protected override void LogOperationSuccess(DeleteCustomerCommand request, DeleteCustomerResult result)
    {
        Logger.LogInformation("Customer deleted successfully with ID: {CustomerId}", request.Id);
    }
}