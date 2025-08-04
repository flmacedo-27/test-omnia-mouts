using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Validator for DeleteBranchCommand that defines business-specific validation rules.
/// </summary>
public class DeleteBranchCommandValidator : AbstractValidator<DeleteBranchCommand>
{
    /// <summary>
    /// Initializes a new instance of the DeleteBranchCommandValidator with business validation rules.
    /// </summary>
    /// <remarks>
    /// Business validation rules include:
    /// - Branch must exist in the system before deletion
    /// </remarks>
    public DeleteBranchCommandValidator(IBranchRepository branchRepository)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, cancellation) => 
                await branchRepository.GetByIdAsync(id, cancellation) != null)
            .WithMessage("Branch not found in the system");
    }
} 