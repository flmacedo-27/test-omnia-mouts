using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

public class UpdateUserHandler : BaseHandler<UpdateUserCommand, UpdateUserResult?, UpdateUserCommandValidator>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserHandler(
        IUserRepository userRepository, 
        IMapper mapper,
        ILogger<UpdateUserHandler> logger,
        UpdateUserCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _userRepository = userRepository;
    }

    protected override async Task<UpdateUserResult?> ExecuteAsync(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        
        user.Update(request.Name, request.Email, request.Phone, request.Password, request.Role, request.Status);
        
        var updatedUser = await _userRepository.UpdateAsync(user, cancellationToken);

        return Mapper.Map<UpdateUserResult>(updatedUser);
    }

    protected override void LogOperationStart(UpdateUserCommand request)
    {
        Logger.LogInformation("Updating user with ID: {UserId}", request.Id);
    }

    protected override void LogOperationSuccess(UpdateUserCommand request, UpdateUserResult? result)
    {
        Logger.LogInformation("User updated successfully with ID: {UserId}", request.Id);
    }
} 