using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

public class GetCustomerHandler : IRequestHandler<GetCustomerCommand, GetCustomerResult?>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCustomerHandler> _logger;
    private readonly GetCustomerCommandValidator _validator;

    public GetCustomerHandler(
        ICustomerRepository customerRepository, 
        IMapper mapper,
        ILogger<GetCustomerHandler> logger,
        GetCustomerCommandValidator validator)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    public async Task<GetCustomerResult?> Handle(GetCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting customer with ID: {CustomerId}", request.Id);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for customer retrieval: {ValidationErrors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

        var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
        
        _logger.LogDebug("Customer retrieved successfully with ID: {CustomerId}", request.Id);
        
        return _mapper.Map<GetCustomerResult>(customer);
    }
} 