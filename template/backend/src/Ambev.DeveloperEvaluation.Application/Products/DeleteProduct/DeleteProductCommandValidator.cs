using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Validator for DeleteProductCommand that defines business-specific validation rules.
/// </summary>
public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the DeleteProductCommandValidator with business validation rules.
    /// </summary>
    /// <remarks>
    /// Business validation rules include:
    /// - Product must exist in the system before deletion
    /// </remarks>
    public DeleteProductCommandValidator(IProductRepository productRepository)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, cancellation) => 
                await productRepository.GetByIdAsync(id, cancellation) != null)
            .WithMessage("Product not found in the system");
    }
} 