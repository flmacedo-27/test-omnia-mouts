using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

/// <summary>
/// Validator for DeleteUserCommand that defines business-specific validation rules.
/// </summary>
public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the DeleteUserCommandValidator with business validation rules.
    /// </summary>
    /// <remarks>
    /// Business validation rules include:
    /// - User must exist in the system before deletion
    /// </remarks>
    public DeleteUserCommandValidator(IUserRepository userRepository)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, cancellation) => 
                await userRepository.GetByIdAsync(id, cancellation) != null)
            .WithMessage("User not found in the system");
    }
}
