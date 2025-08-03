using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;

public class ListCustomersHandler : IRequestHandler<ListCustomersCommand, ListCustomersResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListCustomersHandler> _logger;

    public ListCustomersHandler(
        ICustomerRepository customerRepository, 
        IMapper mapper,
        ILogger<ListCustomersHandler> logger)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ListCustomersResult> Handle(ListCustomersCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listing customers with page {PageNumber} and size {PageSize}", request.PageNumber, request.PageSize);

        var (customers, totalCount) = await _customerRepository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

        _logger.LogInformation("Found {Count} customers, total: {TotalCount}", customers.Count(), totalCount);

        var customerItems = _mapper.Map<IEnumerable<CustomerListItem>>(customers);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        _logger.LogDebug("Returning {CustomerCount} customers with {TotalPages} total pages", customerItems.Count(), totalPages);

        return new ListCustomersResult
        {
            Customers = customerItems,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };
    }
} 