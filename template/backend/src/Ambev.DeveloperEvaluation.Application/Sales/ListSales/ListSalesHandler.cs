using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Handler for listing sales
/// </summary>
public class ListSalesHandler : BaseHandler<ListSalesCommand, ListSalesResult, ListSalesCommandValidator>
{
    private readonly ISaleRepository _saleRepository;

    public ListSalesHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<ListSalesHandler> logger,
        ListSalesCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _saleRepository = saleRepository;
    }

    protected override async Task<ListSalesResult> ExecuteAsync(ListSalesCommand request, CancellationToken cancellationToken)
    {
        var (sales, totalCount) = await _saleRepository.GetAllAsync(
            request.PageNumber, 
            request.PageSize, 
            cancellationToken);

        var saleListItems = Mapper.Map<List<SaleListItem>>(sales);

        return new ListSalesResult(saleListItems, totalCount, request.PageNumber, request.PageSize);
    }

    protected override void LogOperationStart(ListSalesCommand request)
    {
        Logger.LogInformation("Listing sales with page {PageNumber} and size {PageSize}", 
            request.PageNumber, request.PageSize);
    }

    protected override void LogOperationSuccess(ListSalesCommand request, ListSalesResult result)
    {
        if (result != null && result.Items != null)
        {
            Logger.LogInformation("Sales listed successfully. Retrieved {Count} sales", result.Items.Count());
        }
        else
        {
            Logger.LogWarning("Sales listing completed but result or items is null");
        }
    }
} 