using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

public class UpdateCustomerHandler : BaseHandler<UpdateCustomerCommand, UpdateCustomerResult?, UpdateCustomerCommandValidator>
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerHandler(
        ICustomerRepository customerRepository, 
        IMapper mapper,
        ILogger<UpdateCustomerHandler> logger,
        UpdateCustomerCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _customerRepository = customerRepository;
    }

    protected override async Task<UpdateCustomerResult?> ExecuteAsync(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
        
        customer.Update(request.Name, request.Email, request.Phone, request.CustomerType, request.DocumentNumber, request.Active);
        
        var updatedCustomer = await _customerRepository.UpdateAsync(customer, cancellationToken);

        return Mapper.Map<UpdateCustomerResult>(updatedCustomer);
    }

    protected override void LogOperationStart(UpdateCustomerCommand request)
    {
        Logger.LogInformation("Updating customer with ID: {CustomerId}", request.Id);
    }

    protected override void LogOperationSuccess(UpdateCustomerCommand request, UpdateCustomerResult? result)
    {
        if (result != null)
        {
            Logger.LogInformation("Customer updated successfully with ID: {CustomerId}", request.Id);
        }
    }
} 