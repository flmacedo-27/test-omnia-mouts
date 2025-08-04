using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.UpdateBranch;

public class UpdateBranchRequestValidator : AbstractValidator<UpdateBranchRequest>
{
    public UpdateBranchRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Branch name is required")
            .MaximumLength(100)
            .WithMessage("Branch name cannot exceed 100 characters");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Branch code is required")
            .MaximumLength(10)
            .WithMessage("Branch phone cannot be longer than 10 characters");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Branch address is required")
            .MaximumLength(200)
            .WithMessage("Branch address cannot exceed 200 characters");
    }
} 