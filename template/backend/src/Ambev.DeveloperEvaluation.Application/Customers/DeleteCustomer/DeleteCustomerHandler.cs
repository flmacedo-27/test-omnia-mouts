using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, DeleteCustomerResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<DeleteCustomerHandler> _logger;
    private readonly DeleteCustomerCommandValidator _validator;

    public DeleteCustomerHandler(
        ICustomerRepository customerRepository,
        ILogger<DeleteCustomerHandler> logger,
        DeleteCustomerCommandValidator validator)
    {
        _customerRepository = customerRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<DeleteCustomerResult> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting customer with ID: {CustomerId}", request.Id);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for customer deletion: {ValidationErrors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

        await _customerRepository.DeleteAsync(request.Id, cancellationToken);
        
        _logger.LogInformation("Customer deleted successfully with ID: {CustomerId}", request.Id);

        return new DeleteCustomerResult
        {
            Success = true,
            Message = "Customer deleted successfully"
        };
    }
}