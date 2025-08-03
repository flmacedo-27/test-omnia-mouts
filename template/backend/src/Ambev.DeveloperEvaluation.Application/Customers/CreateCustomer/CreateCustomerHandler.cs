using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCustomerHandler> _logger;

    public CreateCustomerHandler(
        ICustomerRepository customerRepository, 
        IMapper mapper,
        ILogger<CreateCustomerHandler> logger)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateCustomerResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating customer with name: {CustomerName}, Email: {Email}", request.Name, request.Email);

        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            CustomerType = request.CustomerType,
            DocumentNumber = request.DocumentNumber,
            Active = true
        };

        var createdCustomer = await _customerRepository.CreateAsync(customer, cancellationToken);

        _logger.LogInformation("Customer created successfully with ID: {CustomerId}", createdCustomer.Id);

        return _mapper.Map<CreateCustomerResult>(createdCustomer);
    }
} 