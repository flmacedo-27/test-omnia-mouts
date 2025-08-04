using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

/// <summary>
/// Validator for GetBranchCommand that defines business-specific validation rules.
/// </summary>
public class GetBranchCommandValidator : AbstractValidator<GetBranchCommand>
{
    /// <summary>
    /// Initializes a new instance of the GetBranchCommandValidator with business validation rules.
    /// </summary>
    /// <remarks>
    /// Business validation rules include:
    /// - Id: Must exist in the system
    /// </remarks>
    public GetBranchCommandValidator(IBranchRepository branchRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Branch ID is required")
            .MustAsync(async (id, cancellation) => 
                await branchRepository.GetByIdAsync(id, cancellation) != null)
            .WithMessage("Branch not found");
    }
} 