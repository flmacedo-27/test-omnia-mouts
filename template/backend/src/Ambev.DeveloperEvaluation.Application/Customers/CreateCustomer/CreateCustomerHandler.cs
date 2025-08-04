using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;

public class CreateCustomerHandler : BaseHandler<CreateCustomerCommand, CreateCustomerResult, CreateCustomerCommandValidator>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerHandler(
        ICustomerRepository customerRepository, 
        IMapper mapper,
        ILogger<CreateCustomerHandler> logger,
        CreateCustomerCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _customerRepository = customerRepository;
    }

    protected override async Task<CreateCustomerResult> ExecuteAsync(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            CustomerType = request.CustomerType,
            DocumentNumber = request.DocumentNumber
        };

        var createdCustomer = await _customerRepository.CreateAsync(customer, cancellationToken);

        return Mapper.Map<CreateCustomerResult>(createdCustomer);
    }

    protected override void LogOperationStart(CreateCustomerCommand request)
    {
        Logger.LogInformation("Creating customer with name: {CustomerName}, Email: {Email}", request.Name, request.Email);
    }

    protected override void LogOperationSuccess(CreateCustomerCommand request, CreateCustomerResult result)
    {
        if (result != null)
        {
            Logger.LogInformation("Customer created successfully with ID: {CustomerId}", result.Id);
        }
        else
        {
            Logger.LogWarning("Customer creation completed but result is null");
        }
    }
} 