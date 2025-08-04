using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

/// <summary>
/// Validator for CreateBranchCommand that defines business-specific validation rules.
/// </summary>
public class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateBranchCommandValidator with business validation rules.
    /// </summary>
    /// <remarks>
    /// Business validation rules include:
    /// - Code: Must not already exist in the system
    /// - Name: Must not already exist in the system
    /// </remarks>
    public CreateBranchCommandValidator(IBranchRepository branchRepository)
    {
        RuleFor(branch => branch.Code)
            .MustAsync(async (code, cancellation) => 
                !await branchRepository.ExistsByCodeAsync(code, cancellation))
            .WithMessage("Branch code already exists in the system");
    }
} 