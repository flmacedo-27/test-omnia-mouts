using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResult?>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateUserHandler> _logger;
    private readonly UpdateUserCommandValidator _validator;

    public UpdateUserHandler(
        IUserRepository userRepository, 
        IMapper mapper,
        ILogger<UpdateUserHandler> logger,
        UpdateUserCommandValidator validator)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    public async Task<UpdateUserResult?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating user with ID: {UserId}", request.Id);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for user update: {ValidationErrors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        
        user.Update(request.Name, request.Email, request.Phone, request.Password, request.Role, request.Status);
        
        var updatedUser = await _userRepository.UpdateAsync(user, cancellationToken);
        
        _logger.LogInformation("User updated successfully with ID: {UserId}", updatedUser.Id);

        return _mapper.Map<UpdateUserResult>(updatedUser);
    }
} 