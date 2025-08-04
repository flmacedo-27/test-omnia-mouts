using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUser;

public class ListUserHandler : BaseHandler<ListUserCommand, ListUserResult, ListUserCommandValidator>
{
    private readonly IUserRepository _userRepository;

    public ListUserHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<ListUserHandler> logger,
        ListUserCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _userRepository = userRepository;
    }

    protected override async Task<ListUserResult> ExecuteAsync(ListUserCommand request, CancellationToken cancellationToken)
    {
        var (users, totalCount) = await _userRepository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

        var userListItems = Mapper.Map<IEnumerable<UserListItem>>(users);

        return new ListUserResult(userListItems, totalCount, request.PageNumber, request.PageSize);
    }

    protected override void LogOperationStart(ListUserCommand request)
    {
        Logger.LogInformation("Listing users with page {PageNumber} and size {PageSize}", request.PageNumber, request.PageSize);
    }

    protected override void LogOperationSuccess(ListUserCommand request, ListUserResult result)
    {
        if (result != null && result.Items != null)
        {
            Logger.LogInformation("Users listed successfully. Retrieved {Count} users", result.Items.Count());
        }
        else
        {
            Logger.LogWarning("Users listing completed but result or items is null");
        }
    }
} 