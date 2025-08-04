using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Validator for GetSaleCommand that defines business-specific validation rules.
/// </summary>
public class GetSaleCommandValidator : AbstractValidator<GetSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the GetSaleCommandValidator with business validation rules.
    /// </summary>
    /// <remarks>
    /// Business validation rules include:
    /// - Id: Must exist in the system
    /// </remarks>
    public GetSaleCommandValidator(ISaleRepository saleRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required")
            .MustAsync(async (id, cancellation) => 
                await saleRepository.GetByIdAsync(id, cancellation) != null)
            .WithMessage("Sale not found");
    }
} 