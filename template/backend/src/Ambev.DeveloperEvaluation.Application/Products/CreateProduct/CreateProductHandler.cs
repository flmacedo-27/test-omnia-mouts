using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductHandler : BaseHandler<CreateProductCommand, CreateProductResult, CreateProductCommandValidator>
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(
        IProductRepository productRepository, 
        IMapper mapper,
        ILogger<CreateProductHandler> logger,
        CreateProductCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _productRepository = productRepository;
    }

    protected override async Task<CreateProductResult> ExecuteAsync(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Code = request.Code,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            SKU = request.SKU
        };

        var createdProduct = await _productRepository.CreateAsync(product, cancellationToken);

        return Mapper.Map<CreateProductResult>(createdProduct);
    }

    protected override void LogOperationStart(CreateProductCommand request)
    {
        Logger.LogInformation("Creating product with name: {ProductName}, SKU: {SKU}", request.Name, request.SKU);
    }

    protected override void LogOperationSuccess(CreateProductCommand request, CreateProductResult result)
    {
        if (result != null)
        {
            Logger.LogInformation("Product created successfully with ID: {ProductId}", result.Id);
        }
        else
        {
            Logger.LogWarning("Product creation completed but result is null");
        }
    }
} 