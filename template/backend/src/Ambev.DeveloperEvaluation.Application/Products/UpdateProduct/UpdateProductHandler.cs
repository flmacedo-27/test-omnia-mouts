using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductHandler> _logger;

    public UpdateProductHandler(
        IProductRepository productRepository, 
        IMapper mapper,
        ILogger<UpdateProductHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UpdateProductResult?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating product with ID: {ProductId}", request.Id);

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (product == null)
        {
            _logger.LogWarning("Product not found for update with ID: {ProductId}", request.Id);
            return null;
        }

        _logger.LogDebug("Product found, updating with new values. Old name: {OldName}, New name: {NewName}", 
            product.Name, request.Name);

        product.Update(request.Name, request.Code, request.Description, request.Price, request.StockQuantity, request.SKU);
        
        var updatedProduct = await _productRepository.UpdateAsync(product, cancellationToken);
        
        _logger.LogInformation("Product updated successfully with ID: {ProductId}", updatedProduct.Id);

        return _mapper.Map<UpdateProductResult>(updatedProduct);
    }
} 