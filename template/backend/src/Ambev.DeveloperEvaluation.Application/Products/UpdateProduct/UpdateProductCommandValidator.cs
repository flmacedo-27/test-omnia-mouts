using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Validator for UpdateProductCommand that defines business-specific validation rules.
/// </summary>
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateProductCommandValidator with business validation rules.
    /// </summary>
    /// <remarks>
    /// Business validation rules include:
    /// - Product must exist in the system
    /// - New code must not conflict with existing products (excluding current product)
    /// - New SKU must not conflict with existing products (excluding current product)
    /// </remarks>
    public UpdateProductCommandValidator(IProductRepository productRepository)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, cancellation) => 
                await productRepository.GetByIdAsync(id, cancellation) != null)
            .WithMessage("Product not found in the system");

        RuleFor(command => command.Code)
            .MustAsync(async (command, code, cancellation) => 
            {
                var existingProduct = await productRepository.GetByIdAsync(command.Id, cancellation);
                if (existingProduct == null) return true;

                var productWithSameCode = await productRepository.GetByCodeAsync(code, cancellation);
                return productWithSameCode == null || productWithSameCode.Id == command.Id;
            })
            .WithMessage("Product code already exists in another product");

        RuleFor(command => command.SKU)
            .MustAsync(async (command, sku, cancellation) => 
            {
                var existingProduct = await productRepository.GetByIdAsync(command.Id, cancellation);
                if (existingProduct == null) return true;

                var productWithSameSKU = await productRepository.GetBySKUAsync(sku, cancellation);
                return productWithSameSKU == null || productWithSameSKU.Id == command.Id;
            })
            .WithMessage("Product SKU already exists in another product");
    }
} 