using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for cancelling a sale
/// </summary>
public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleHandler> _logger;
    private readonly IMediator _mediator;
    private readonly CancelSaleCommandValidator _validator;

    public CancelSaleHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<CancelSaleHandler> logger,
        IMediator mediator,
        CancelSaleCommandValidator validator)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
        _validator = validator;
    }

    public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cancelling sale with ID {SaleId}", request.Id);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for sale cancelation: {ValidationErrors}",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

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

        _logger.LogInformation("Sale {SaleNumber} cancelled successfully", sale.SaleNumber);

        var saleCancelledEvent = new SaleCancelledEvent(
            updatedSale.Id,
            updatedSale.SaleNumber,
            updatedSale.CancellationReason!,
            updatedSale.CancelledAt!.Value);

        await _mediator.Publish(saleCancelledEvent, cancellationToken);

        return _mapper.Map<CancelSaleResult>(updatedSale);
    }
} 