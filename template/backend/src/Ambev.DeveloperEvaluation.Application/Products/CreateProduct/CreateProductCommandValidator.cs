using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Validator for CreateProductCommand that defines business-specific validation rules.
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateProductCommandValidator with business validation rules.
    /// </summary>
    /// <remarks>
    /// Business validation rules include:
    /// - Code: Must not already exist in the system
    /// - SKU: Must not already exist in the system
    /// </remarks>
    public CreateProductCommandValidator(IProductRepository productRepository)
    {
        RuleFor(product => product.Code)
            .MustAsync(async (code, cancellation) => 
                !await productRepository.ExistsByCodeAsync(code, cancellation))
            .WithMessage("Product code already exists in the system");

        RuleFor(product => product.SKU)
            .MustAsync(async (sku, cancellation) => 
                !await productRepository.ExistsBySKUAsync(sku, cancellation))
            .WithMessage("Product SKU already exists in the system");
    }
} 