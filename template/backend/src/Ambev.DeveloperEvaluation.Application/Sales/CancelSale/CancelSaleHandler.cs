using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for cancelling a sale
/// </summary>
public class CancelSaleHandler : BaseHandler<CancelSaleCommand, CancelSaleResult, CancelSaleCommandValidator>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;

    public CancelSaleHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<CancelSaleHandler> logger,
        IMediator mediator,
        CancelSaleCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _mediator = mediator;
    }

    protected override async Task<CancelSaleResult> ExecuteAsync(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (sale.Status == SaleStatus.Cancelled)
            throw new InvalidOperationException($"Sale {sale.SaleNumber} is already cancelled");

        sale.Cancel(request.Reason);

        foreach (var item in sale.Items.Where(i => i.Status == SaleItemStatus.Active))
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product != null)
            {
                product.StockQuantity += item.Quantity;
                await _productRepository.UpdateAsync(product, cancellationToken);
            }
        }

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        var saleCancelledEvent = new SaleCancelledEvent(
            updatedSale.Id,
            updatedSale.SaleNumber,
            updatedSale.CancellationReason!,
            updatedSale.CancelledAt!.Value);

        await _mediator.Publish(saleCancelledEvent, cancellationToken);

        return Mapper.Map<CancelSaleResult>(updatedSale);
    }

    protected override void LogOperationStart(CancelSaleCommand request)
    {
        Logger.LogInformation("Cancelling sale with ID {SaleId}", request.Id);
    }

    protected override void LogOperationSuccess(CancelSaleCommand request, CancelSaleResult result)
    {
        if (result != null)
        {
            Logger.LogInformation("Sale {SaleNumber} cancelled successfully", result.SaleNumber);
        }
        else
        {
            Logger.LogWarning("Sale cancellation completed but result is null");
        }
    }
} 