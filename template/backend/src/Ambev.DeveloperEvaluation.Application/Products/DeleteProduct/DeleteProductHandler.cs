using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<DeleteProductHandler> _logger;

    public DeleteProductHandler(
        IProductRepository productRepository,
        ILogger<DeleteProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting product with ID: {ProductId}", request.Id);

        var success = await _productRepository.DeleteAsync(request.Id, cancellationToken);
        
        if (!success)
        {
            _logger.LogWarning("Product not found for deletion with ID: {ProductId}", request.Id);
            return new DeleteProductResult
            {
                Success = false,
                Message = "Product not found"
            };
        }

        _logger.LogInformation("Product deleted successfully with ID: {ProductId}", request.Id);

        return new DeleteProductResult
        {
            Success = true,
            Message = "Product deleted successfully"
        };
    }
} 