using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;

public class ListCustomersHandler : BaseHandler<ListCustomersCommand, ListCustomersResult, ListCustomersCommandValidator>
{
    private readonly ICustomerRepository _customerRepository;

    public ListCustomersHandler(
        ICustomerRepository customerRepository, 
        IMapper mapper,
        ILogger<ListCustomersHandler> logger,
        ListCustomersCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _customerRepository = customerRepository;
    }

    protected override async Task<ListCustomersResult> ExecuteAsync(ListCustomersCommand request, CancellationToken cancellationToken)
    {
        var (customers, totalCount) = await _customerRepository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

        var customerItems = Mapper.Map<IEnumerable<CustomerListItem>>(customers);

        return new ListCustomersResult(customerItems, totalCount, request.PageNumber, request.PageSize);
    }

    protected override void LogOperationStart(ListCustomersCommand request)
    {
        Logger.LogInformation("Listing customers with page {PageNumber} and size {PageSize}", request.PageNumber, request.PageSize);
    }

    protected override void LogOperationSuccess(ListCustomersCommand request, ListCustomersResult result)
    {
        if (result != null && result.Items != null)
        {
            Logger.LogInformation("Customers listed successfully. Retrieved {Count} customers", result.Items.Count());
        }
        else
        {
            Logger.LogWarning("Customers listing completed but result or items is null");
        }
    }
} 