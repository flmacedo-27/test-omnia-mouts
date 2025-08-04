using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Handler for getting a sale by ID
/// </summary>
public class GetSaleHandler : BaseHandler<GetSaleCommand, GetSaleResult, GetSaleCommandValidator>
{
    private readonly ISaleRepository _saleRepository;

    public GetSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<GetSaleHandler> logger,
        GetSaleCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _saleRepository = saleRepository;
    }

    protected override async Task<GetSaleResult> ExecuteAsync(GetSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        
        var result = Mapper.Map<GetSaleResult>(sale);
        
        return result;
    }

    protected override void LogOperationStart(GetSaleCommand request)
    {
        Logger.LogInformation("Getting sale with ID {SaleId}", request.Id);
    }

    protected override void LogOperationSuccess(GetSaleCommand request, GetSaleResult result)
    {
        if (result != null)
        {
            Logger.LogInformation("Sale {SaleNumber} retrieved successfully", result.SaleNumber);
        }
        else
        {
            Logger.LogWarning("Sale retrieval completed but result is null");
        }
    }
} 