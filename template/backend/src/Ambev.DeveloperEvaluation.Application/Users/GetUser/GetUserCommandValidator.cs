using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

public class GetUserCommandValidator : AbstractValidator<GetUserCommand>
{
    public GetUserCommandValidator(IUserRepository userRepository)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, cancellation) => 
                await userRepository.GetByIdAsync(id, cancellation) != null)
            .WithMessage("User not found in the system");
    }
} 