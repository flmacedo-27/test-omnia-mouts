using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductHandler : BaseHandler<UpdateProductCommand, UpdateProductResult?, UpdateProductCommandValidator>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductHandler(
        IProductRepository productRepository, 
        IMapper mapper,
        ILogger<UpdateProductHandler> logger,
        UpdateProductCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _productRepository = productRepository;
    }

    protected override async Task<UpdateProductResult?> ExecuteAsync(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        
        product.Update(request.Name, request.Code, request.Description, request.Price, request.StockQuantity, request.SKU);
        
        var updatedProduct = await _productRepository.UpdateAsync(product, cancellationToken);

        return Mapper.Map<UpdateProductResult>(updatedProduct);
    }

    protected override void LogOperationStart(UpdateProductCommand request)
    {
        Logger.LogInformation("Updating product with ID: {ProductId}", request.Id);
    }

    protected override void LogOperationSuccess(UpdateProductCommand request, UpdateProductResult? result)
    {
        if (result != null)
        {
            Logger.LogInformation("Product updated successfully with ID: {ProductId}", request.Id);
        }
    }
} 