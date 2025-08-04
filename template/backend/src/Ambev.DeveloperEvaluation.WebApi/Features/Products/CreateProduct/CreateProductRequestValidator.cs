using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(100)
            .WithMessage("Product name cannot be longer than 100 characters");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Product code is required")
            .MaximumLength(100)
            .WithMessage("Product code cannot be longer than 10 characters");


        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Product description cannot be longer than 500 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Product price must be greater than zero");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock quantity must be greater than or equal to zero");

        RuleFor(x => x.SKU)
            .NotEmpty()
            .WithMessage("Product SKU is required")
            .MaximumLength(50)
            .WithMessage("Product SKU cannot be longer than 50 characters");
    }
} 