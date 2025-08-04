using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for creating a new sale
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly IMediator _mediator;
    private readonly CreateSaleCommandValidator _validator;

    public CreateSaleHandler(
        ISaleRepository saleRepository,
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<CreateSaleHandler> logger,
        IMediator mediator,
        CreateSaleCommandValidator validator)
    {
        _saleRepository = saleRepository;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
        _validator = validator;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating sale for customer {CustomerId} at branch {BranchId}", request.CustomerId, request.BranchId);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for sale creation: {ValidationErrors}",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

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

        _logger.LogInformation("Sale {SaleNumber} created successfully with total amount {TotalAmount}", 
            createdSale.SaleNumber, createdSale.TotalAmount);

        var saleCreatedEvent = new SaleCreatedEvent(
            createdSale.Id,
            createdSale.SaleNumber,
            createdSale.CustomerId,
            createdSale.BranchId,
            createdSale.TotalAmount,
            createdSale.CreatedAt);

        await _mediator.Publish(saleCreatedEvent, cancellationToken);

        return _mapper.Map<CreateSaleResult>(createdSale);
    }
} 