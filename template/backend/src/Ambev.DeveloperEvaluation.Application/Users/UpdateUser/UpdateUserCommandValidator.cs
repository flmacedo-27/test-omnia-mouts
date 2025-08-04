using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Validator for UpdateUserCommand that defines business-specific validation rules.
/// </summary>
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateUserCommandValidator with business validation rules.
    /// </summary>
    /// <remarks>
    /// Business validation rules include:
    /// - User must exist in the system
    /// - New email must not conflict with existing users (excluding current user)
    /// - Status: Cannot be set to Unknown
    /// - Role: Cannot be set to None
    /// </remarks>
    public UpdateUserCommandValidator(IUserRepository userRepository)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, cancellation) => 
                await userRepository.GetByIdAsync(id, cancellation) != null)
            .WithMessage("User not found in the system");

        RuleFor(command => command.Email)
            .MustAsync(async (command, email, cancellation) => 
            {
                var existingUser = await userRepository.GetByIdAsync(command.Id, cancellation);
                if (existingUser == null) return true;

                var userWithSameEmail = await userRepository.GetByEmailAsync(email, cancellation);
                return userWithSameEmail == null || userWithSameEmail.Id == command.Id;
            })
            .WithMessage("User email already exists in another user");

        RuleFor(command => command.Status)
            .NotEqual(UserStatus.Unknown)
            .WithMessage("User status cannot be Unknown");

        RuleFor(command => command.Role)
            .NotEqual(UserRole.None)
            .WithMessage("User role cannot be None");
    }
} 