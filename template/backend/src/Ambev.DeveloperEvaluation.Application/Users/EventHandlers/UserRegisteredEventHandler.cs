using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.EventHandlers;

/// <summary>
/// Handler for UserRegisteredEvent that processes user registration events
/// </summary>
public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
{
    private readonly ILogger<UserRegisteredEventHandler> _logger;

    public UserRegisteredEventHandler(ILogger<UserRegisteredEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "User registered event received - UserId: {UserId}, Email: {Email}, Username: {Username}, CreatedAt: {CreatedAt}",
            notification.User.Id,
            notification.User.Email,
            notification.User.Username,
            notification.User.CreatedAt);

        await Task.CompletedTask;
    }
} 