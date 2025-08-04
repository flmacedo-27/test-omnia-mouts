using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

/// <summary>
/// Handler for processing DeleteUserCommand requests
/// </summary>
public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<DeleteUserHandler> _logger;
    private readonly DeleteUserCommandValidator _validator;

    /// <summary>
    /// Initializes a new instance of DeleteUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="logger">The logger instance</param>
    /// <param name="validator">The validator for DeleteUserCommand</param>
    public DeleteUserHandler(
        IUserRepository userRepository,
        ILogger<DeleteUserHandler> logger,
        DeleteUserCommandValidator validator)
    {
        _userRepository = userRepository;
        _logger = logger;
        _validator = validator;
    }

    /// <summary>
    /// Handles the DeleteUserCommand request
    /// </summary>
    /// <param name="request">The DeleteUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the delete operation</returns>
    public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting user with ID: {UserId}", request.Id);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for user deletion: {ValidationErrors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

        await _userRepository.DeleteAsync(request.Id, cancellationToken);
        
        _logger.LogInformation("User deleted successfully with ID: {UserId}", request.Id);

        return new DeleteUserResponse { Success = true };
    }
}
