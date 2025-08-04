using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductCommandValidator : AbstractValidator<GetProductCommand>
{
    public GetProductCommandValidator(IProductRepository productRepository)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, cancellation) => 
                await productRepository.GetByIdAsync(id, cancellation) != null)
            .WithMessage("Product not found in the system");
    }
} 