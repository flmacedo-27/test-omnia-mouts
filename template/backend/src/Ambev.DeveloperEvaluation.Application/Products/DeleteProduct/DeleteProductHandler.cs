using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

public class DeleteProductHandler : BaseHandler<DeleteProductCommand, DeleteProductResult, DeleteProductCommandValidator>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<DeleteProductHandler> logger,
        DeleteProductCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _productRepository = productRepository;
    }

    protected override async Task<DeleteProductResult> ExecuteAsync(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await _productRepository.DeleteAsync(request.Id, cancellationToken);

        return new DeleteProductResult
        {
            Success = true,
            Message = "Product deleted successfully"
        };
    }

    protected override void LogOperationStart(DeleteProductCommand request)
    {
        Logger.LogInformation("Deleting product with ID: {ProductId}", request.Id);
    }

    protected override void LogOperationSuccess(DeleteProductCommand request, DeleteProductResult result)
    {
        Logger.LogInformation("Product deleted successfully with ID: {ProductId}", request.Id);
    }
} 