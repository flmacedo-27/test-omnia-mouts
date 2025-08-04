using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for creating a new sale
/// </summary>
public class CreateSaleHandler : BaseHandler<CreateSaleCommand, CreateSaleResult, CreateSaleCommandValidator>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;

    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<CreateSaleHandler> logger,
        IMediator mediator,
        CreateSaleCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _mediator = mediator;
    }

    protected override async Task<CreateSaleResult> ExecuteAsync(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product == null)
                throw new InvalidOperationException($"Product with ID {item.ProductId} not found");

            if (product.StockQuantity < item.Quantity)
                throw new InvalidOperationException($"Insufficient stock for product {product.Name}. Available: {product.StockQuantity}, Requested: {item.Quantity}");

            if (item.Quantity > 20)
                throw new InvalidOperationException($"Cannot sell more than 20 identical items. Requested: {item.Quantity}");
        }

        var sale = new Sale
        {
            SaleNumber = await _saleRepository.GenerateSaleNumberAsync(cancellationToken),
            SaleDate = DateTime.UtcNow,
            CustomerId = request.CustomerId,
            BranchId = request.BranchId
        };

        foreach (var itemCommand in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemCommand.ProductId, cancellationToken)!;
            
            var saleItem = new SaleItem
            {
                SaleId = sale.Id,
                ProductId = itemCommand.ProductId,
                Quantity = itemCommand.Quantity,
                UnitPrice = itemCommand.UnitPrice
            };

            saleItem.CalculateDiscount();

            product.StockQuantity -= itemCommand.Quantity;
            await _productRepository.UpdateAsync(product, cancellationToken);

            sale.Items.Add(saleItem);
        }

        sale.CalculateTotal();

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        var saleCreatedEvent = new SaleCreatedEvent(
            createdSale.Id,
            createdSale.SaleNumber,
            createdSale.CustomerId,
            createdSale.BranchId,
            createdSale.TotalAmount,
            createdSale.CreatedAt);

        await _mediator.Publish(saleCreatedEvent, cancellationToken);

        return Mapper.Map<CreateSaleResult>(createdSale);
    }

    protected override void LogOperationStart(CreateSaleCommand request)
    {
        Logger.LogInformation("Creating sale for customer {CustomerId} at branch {BranchId}", request.CustomerId, request.BranchId);
    }

    protected override void LogOperationSuccess(CreateSaleCommand request, CreateSaleResult result)
    {
        Logger.LogInformation("Sale {SaleNumber} created successfully with total amount {TotalAmount}", 
            result.SaleNumber, result.TotalAmount);
    }
} 