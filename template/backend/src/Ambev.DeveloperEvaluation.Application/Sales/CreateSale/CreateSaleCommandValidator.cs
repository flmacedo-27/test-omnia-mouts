using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand that defines business-specific validation rules.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleCommandValidator with business validation rules.
    /// </summary>
    /// <remarks>
    /// Business validation rules include:
    /// - CustomerId: Must exist in the system
    /// - BranchId: Must exist in the system
    /// - Items: Must not be empty
    /// - Each item's ProductId: Must exist in the system
    /// </remarks>
    public CreateSaleCommandValidator(
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IProductRepository productRepository)
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required")
            .MustAsync(async (customerId, cancellation) => 
            {
                var customer = await customerRepository.GetByIdAsync(customerId, cancellation);
                return customer != null && customer.Active;
            })
            .WithMessage("Customer not found or inactive");

        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("Branch ID is required")
            .MustAsync(async (branchId, cancellation) => 
                await branchRepository.GetByIdAsync(branchId, cancellation) != null)
            .WithMessage("Branch not found");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one item is required");

        RuleForEach(x => x.Items)
            .SetValidator(new CreateSaleItemCommandValidator(productRepository));
    }
}

/// <summary>
/// Validator for CreateSaleItemCommand
/// </summary>
public class CreateSaleItemCommandValidator : AbstractValidator<CreateSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleItemCommandValidator
    /// </summary>
    public CreateSaleItemCommandValidator(IProductRepository productRepository)
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required")
            .MustAsync(async (productId, cancellation) => 
            {
                var product = await productRepository.GetByIdAsync(productId, cancellation);
                return product != null && product.Active;
            })
            .WithMessage("Product not found or inactive");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .LessThanOrEqualTo(20)
            .WithMessage("Quantity must be between 1 and 20");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than 0");
    }
} 