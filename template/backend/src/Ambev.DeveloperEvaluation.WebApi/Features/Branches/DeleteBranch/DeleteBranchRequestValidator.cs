using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.DeleteBranch;

public class DeleteBranchRequestValidator : AbstractValidator<DeleteBranchRequest>
{
    public DeleteBranchRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Branch ID is required");
    }
} 