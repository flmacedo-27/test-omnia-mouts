using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Handler for listing sales
/// </summary>
public class ListSalesHandler : IRequestHandler<ListSalesCommand, ListSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListSalesHandler> _logger;

    public ListSalesHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<ListSalesHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ListSalesResult> Handle(ListSalesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listing sales with page {PageNumber} and size {PageSize}", 
            request.PageNumber, request.PageSize);

        var (sales, totalCount) = await _saleRepository.GetAllAsync(
            request.PageNumber, 
            request.PageSize, 
            cancellationToken);

        var saleListItems = _mapper.Map<List<SaleListItem>>(sales);

        _logger.LogInformation("Found {Count} sales out of {TotalCount} total", 
            saleListItems.Count, totalCount);

        return new ListSalesResult(saleListItems, totalCount, request.PageNumber, request.PageSize);
    }
} 