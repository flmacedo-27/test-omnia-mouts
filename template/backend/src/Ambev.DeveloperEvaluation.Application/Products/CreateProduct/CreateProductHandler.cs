using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(
        IProductRepository productRepository, 
        IMapper mapper,
        ILogger<CreateProductHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating product with name: {ProductName}, SKU: {SKU}", request.Name, request.SKU);

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

        _logger.LogInformation("Product created successfully with ID: {ProductId}", createdProduct.Id);

        return _mapper.Map<CreateProductResult>(createdProduct);
    }
} 