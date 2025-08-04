using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

/// <summary>
/// Handler for processing GetUserCommand requests
/// </summary>
public class GetUserHandler : IRequestHandler<GetUserCommand, GetUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUserHandler> _logger;
    private readonly GetUserCommandValidator _validator;

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
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    /// <summary>
    /// Handles the GetUserCommand request
    /// </summary>
    /// <param name="request">The GetUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user details if found</returns>
    public async Task<GetUserResult> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting user with ID: {UserId}", request.Id);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for user retrieval: {ValidationErrors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        
        _logger.LogDebug("User retrieved successfully with ID: {UserId}", request.Id);
        
        return _mapper.Map<GetUserResult>(user);
    }
}
