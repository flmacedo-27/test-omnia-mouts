using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser;

/// <summary>
/// Handler for processing CreateUserCommand requests
/// </summary>
public class CreateUserHandler : BaseHandler<CreateUserCommand, CreateUserResult, CreateUserCommandValidator>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of CreateUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="passwordHasher">The password hasher</param>
    /// <param name="validator">The validator for CreateUserCommand</param>
    /// <param name="logger">The logger instance</param>
    /// <param name="mediator">The mediator instance</param>
    public CreateUserHandler(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher, CreateUserCommandValidator validator, ILogger<CreateUserHandler> logger, IMediator mediator)
        : base(mapper, logger, validator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _mediator = mediator;
    }

    /// <summary>
    /// Handles the CreateUserCommand request
    /// </summary>
    /// <param name="command">The CreateUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created user details</returns>
    protected override async Task<CreateUserResult> ExecuteAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = Mapper.Map<User>(command);
        user.Password = _passwordHasher.HashPassword(command.Password);

        var createdUser = await _userRepository.CreateAsync(user, cancellationToken);

        var userRegisteredEvent = new UserRegisteredEvent(createdUser);
        await _mediator.Publish(userRegisteredEvent, cancellationToken);

        return Mapper.Map<CreateUserResult>(createdUser);
    }

    protected override void LogOperationStart(CreateUserCommand request)
    {
        Logger.LogInformation("Creating user with email: {Email}", request.Email);
    }

    protected override void LogOperationSuccess(CreateUserCommand request, CreateUserResult result)
    {
        if (result != null)
        {
            Logger.LogInformation("User created successfully with ID: {UserId}", result.Id);
        }
        else
        {
            Logger.LogWarning("User creation completed but result is null");
        }
    }
}
