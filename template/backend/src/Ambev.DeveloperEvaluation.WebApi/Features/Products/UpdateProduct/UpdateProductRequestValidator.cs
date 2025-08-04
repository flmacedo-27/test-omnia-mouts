using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(100)
            .WithMessage("Product name cannot exceed 100 characters");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Product code is required")
            .MaximumLength(10)
            .WithMessage("Product code cannot exceed 10 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Product description is required")
            .MaximumLength(500)
            .WithMessage("Product description cannot exceed 500 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Product price must be greater than zero");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock quantity cannot be negative");

        RuleFor(x => x.SKU)
            .NotEmpty()
            .WithMessage("Product SKU is required")
            .MaximumLength(20)
            .WithMessage("Product SKU cannot exceed 20 characters");
    }
} 