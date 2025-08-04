using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<DeleteProductHandler> _logger;
    private readonly DeleteProductCommandValidator _validator;

    public DeleteProductHandler(
        IProductRepository productRepository,
        ILogger<DeleteProductHandler> logger,
        DeleteProductCommandValidator validator)
    {
        _productRepository = productRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting product with ID: {ProductId}", request.Id);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for product deletion: {ValidationErrors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

        await _productRepository.DeleteAsync(request.Id, cancellationToken);
        
        _logger.LogInformation("Product deleted successfully with ID: {ProductId}", request.Id);

        return new DeleteProductResult
        {
            Success = true,
            Message = "Product deleted successfully"
        };
    }
} 