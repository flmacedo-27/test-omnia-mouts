using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

/// <summary>
/// Handler for processing GetUserCommand requests
/// </summary>
public class GetUserHandler : BaseHandler<GetUserCommand, GetUserResult, GetUserCommandValidator>
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of GetUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    /// <param name="validator">The validator for GetUserCommand</param>
    public GetUserHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<GetUserHandler> logger,
        GetUserCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Handles the GetUserCommand request
    /// </summary>
    /// <param name="request">The GetUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user details if found</returns>
    protected override async Task<GetUserResult> ExecuteAsync(GetUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        
        return Mapper.Map<GetUserResult>(user);
    }

    protected override void LogOperationStart(GetUserCommand request)
    {
        Logger.LogInformation("Getting user with ID: {UserId}", request.Id);
    }

    protected override void LogOperationSuccess(GetUserCommand request, GetUserResult result)
    {
        Logger.LogInformation("User retrieved successfully with ID: {UserId}", request.Id);
    }
}
