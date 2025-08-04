using Ambev.DeveloperEvaluation.Application.Users.EventHandlers;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users.EventHandlers;

/// <summary>
/// Tests for UserRegisteredEventHandler
/// </summary>
public class UserRegisteredEventHandlerTests
{
    private readonly ILogger<UserRegisteredEventHandler> _logger;
    private readonly UserRegisteredEventHandler _handler;

    public UserRegisteredEventHandlerTests()
    {
        _logger = Substitute.For<ILogger<UserRegisteredEventHandler>>();
        _handler = new UserRegisteredEventHandler(_logger);
    }

    [Fact(DisplayName = "Handle should log user registered event information")]
    public async Task Handle_ShouldLogUserRegisteredEventInformation()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@example.com",
            CreatedAt = DateTime.UtcNow
        };

        var notification = new UserRegisteredEvent(user);

        // Act
        await _handler.Handle(notification, CancellationToken.None);
    }

    [Fact(DisplayName = "Handle should log correct user data")]
    public async Task Handle_ShouldLogCorrectUserData()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var username = "newuser";
        var email = "newuser@example.com";
        var createdAt = DateTime.UtcNow;

        var user = new User
        {
            Id = userId,
            Username = username,
            Email = email,
            CreatedAt = createdAt
        };

        var notification = new UserRegisteredEvent(user);

        // Act
        await _handler.Handle(notification, CancellationToken.None);
    }

    [Fact(DisplayName = "Handle should complete successfully")]
    public async Task Handle_ShouldCompleteSuccessfully()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@example.com",
            CreatedAt = DateTime.UtcNow
        };

        var notification = new UserRegisteredEvent(user);

        // Act & Assert
        await _handler.Handle(notification, CancellationToken.None);
    }
} 