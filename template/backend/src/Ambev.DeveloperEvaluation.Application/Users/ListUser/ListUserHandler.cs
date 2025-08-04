using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUser;

public class ListUserHandler : IRequestHandler<ListUserCommand, ListUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListUserHandler> _logger;

    public ListUserHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<ListUserHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ListUserResult> Handle(ListUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listing users with page {PageNumber} and size {PageSize}", request.PageNumber, request.PageSize);

        var (users, totalCount) = await _userRepository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

        var userListItems = _mapper.Map<IEnumerable<UserListItem>>(users);

        _logger.LogDebug("Found {UserCount} users out of {TotalCount} total", userListItems.Count(), totalCount);

        return new ListUserResult(userListItems, totalCount, request.PageNumber, request.PageSize);
    }
} 