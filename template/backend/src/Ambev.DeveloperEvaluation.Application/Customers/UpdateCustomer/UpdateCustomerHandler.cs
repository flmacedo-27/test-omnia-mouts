using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, UpdateCustomerResult?>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateCustomerHandler> _logger;
    private readonly UpdateCustomerCommandValidator _validator;

    public UpdateCustomerHandler(
        ICustomerRepository customerRepository, 
        IMapper mapper,
        ILogger<UpdateCustomerHandler> logger,
        UpdateCustomerCommandValidator validator)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    public async Task<UpdateCustomerResult?> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating customer with ID: {CustomerId}", request.Id);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for customer update: {ValidationErrors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

        var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (customer == null)
        {
            _logger.LogWarning("Customer not found for update with ID: {CustomerId}", request.Id);
            return null;
        }

        _logger.LogDebug("Customer found, updating with new values. Old name: {OldName}, New name: {NewName}", 
            customer.Name, request.Name);

        customer.Update(request.Name, request.Email, request.Phone, request.CustomerType, request.DocumentNumber, request.Active);
        
        var updatedCustomer = await _customerRepository.UpdateAsync(customer, cancellationToken);
        
        _logger.LogInformation("Customer updated successfully with ID: {CustomerId}", updatedCustomer.Id);

        return _mapper.Map<UpdateCustomerResult>(updatedCustomer);
    }
} 