using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductHandler> _logger;
    private readonly UpdateProductCommandValidator _validator;

    public UpdateProductHandler(
        IProductRepository productRepository, 
        IMapper mapper,
        ILogger<UpdateProductHandler> logger,
        UpdateProductCommandValidator validator)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    public async Task<UpdateProductResult?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating product with ID: {ProductId}", request.Id);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for product update: {ValidationErrors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        
        product.Update(request.Name, request.Code, request.Description, request.Price, request.StockQuantity, request.SKU);
        
        var updatedProduct = await _productRepository.UpdateAsync(product, cancellationToken);
        
        _logger.LogInformation("Product updated successfully with ID: {ProductId}", updatedProduct.Id);

        return _mapper.Map<UpdateProductResult>(updatedProduct);
    }
} 