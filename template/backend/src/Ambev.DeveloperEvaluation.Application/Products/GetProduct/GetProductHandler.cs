using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductHandler : IRequestHandler<GetProductCommand, GetProductResult?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductHandler> _logger;

    public GetProductHandler(
        IProductRepository productRepository, 
        IMapper mapper,
        ILogger<GetProductHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetProductResult?> Handle(GetProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting product with ID: {ProductId}", request.Id);

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (product == null)
        {
            _logger.LogWarning("Product not found with ID: {ProductId}", request.Id);
            return null;
        }

        _logger.LogDebug("Product found with ID: {ProductId}, Name: {ProductName}", request.Id, product.Name);

        return _mapper.Map<GetProductResult>(product);
    }
} 