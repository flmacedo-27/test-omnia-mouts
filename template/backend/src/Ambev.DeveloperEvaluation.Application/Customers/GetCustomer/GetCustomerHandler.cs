using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

public class GetCustomerHandler : BaseHandler<GetCustomerCommand, GetCustomerResult?, GetCustomerCommandValidator>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerHandler(
        ICustomerRepository customerRepository, 
        IMapper mapper,
        ILogger<GetCustomerHandler> logger,
        GetCustomerCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _customerRepository = customerRepository;
    }

    protected override async Task<GetCustomerResult?> ExecuteAsync(GetCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
        
        return Mapper.Map<GetCustomerResult>(customer);
    }

    protected override void LogOperationStart(GetCustomerCommand request)
    {
        Logger.LogInformation("Getting customer with ID: {CustomerId}", request.Id);
    }

    protected override void LogOperationSuccess(GetCustomerCommand request, GetCustomerResult? result)
    {
        Logger.LogInformation("Customer retrieved successfully with ID: {CustomerId}", request.Id);
    }
} 