using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// Validator for UpdateBranchCommand that defines business-specific validation rules.
/// </summary>
public class UpdateBranchCommandValidator : AbstractValidator<UpdateBranchCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateBranchCommandValidator with business validation rules.
    /// </summary>
    /// <remarks>
    /// Business validation rules include:
    /// - Branch must exist in the system
    /// - New code must not conflict with existing branches (excluding current branch)
    /// </remarks>
    public UpdateBranchCommandValidator(IBranchRepository branchRepository)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, cancellation) => 
                await branchRepository.GetByIdAsync(id, cancellation) != null)
            .WithMessage("Branch not found in the system");

        RuleFor(command => command.Code)
            .MustAsync(async (command, code, cancellation) => 
            {
                var existingBranch = await branchRepository.GetByIdAsync(command.Id, cancellation);
                if (existingBranch == null) return true;

                var branchWithSameCode = await branchRepository.GetByCodeAsync(code, cancellation);
                return branchWithSameCode == null || branchWithSameCode.Id == command.Id;
            })
            .WithMessage("Branch code already exists in another branch");
    }
} 