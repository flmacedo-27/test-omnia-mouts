using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

/// <summary>
/// Handler for processing DeleteUserCommand requests
/// </summary>
public class DeleteUserHandler : BaseHandler<DeleteUserCommand, DeleteUserResponse, DeleteUserCommandValidator>
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of DeleteUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    /// <param name="validator">The validator for DeleteUserCommand</param>
    public DeleteUserHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<DeleteUserHandler> logger,
        DeleteUserCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Handles the DeleteUserCommand request
    /// </summary>
    /// <param name="request">The DeleteUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the delete operation</returns>
    protected override async Task<DeleteUserResponse> ExecuteAsync(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.DeleteAsync(request.Id, cancellationToken);

        return new DeleteUserResponse { Success = true };
    }

    protected override void LogOperationStart(DeleteUserCommand request)
    {
        Logger.LogInformation("Deleting user with ID: {UserId}", request.Id);
    }

    protected override void LogOperationSuccess(DeleteUserCommand request, DeleteUserResponse result)
    {
        Logger.LogInformation("User deleted successfully with ID: {UserId}", request.Id);
    }
}
