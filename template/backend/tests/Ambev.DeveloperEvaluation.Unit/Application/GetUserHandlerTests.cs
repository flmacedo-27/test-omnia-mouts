using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="GetUserHandler"/> class.
/// </summary>
public class GetUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUserHandler> _logger;
    private readonly GetUserCommandValidator _validator;
    private readonly GetUserHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserHandlerTests"/> class.
    /// </summary>
    public GetUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetUserHandler>>();
        _validator = new GetUserCommandValidator(_userRepository);
        _handler = new GetUserHandler(_userRepository, _mapper, _logger, _validator);
    }

    /// <summary>
    /// Tests that a valid user retrieval request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid user ID When getting user Then returns user data")]
    public async Task Handle_ValidRequest_ReturnsUserData()
    {
        // Given
        var command = GetUserHandlerTestData.GenerateValidCommand();
        var userId = command.Id;

        var user = GetUserHandlerTestData.GenerateUserWithId(userId);
        var expectedResult = GetUserHandlerTestData.GenerateResultFromUser(user);

        _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<GetUserResult>(user).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _userRepository.Received(2).GetByIdAsync(userId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetUserResult>(user);
    }

    /// <summary>
    /// Tests that an invalid user retrieval request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid user ID When getting user Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = GetUserHandlerTestData.GenerateInvalidCommand();

        _userRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((User?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that user retrieval works for active users.
    /// </summary>
    [Fact(DisplayName = "Given active user When getting user Then returns user data")]
    public async Task Handle_ActiveUser_ReturnsUserData()
    {
        // Given
        var command = GetUserHandlerTestData.GenerateValidCommand();
        var userId = command.Id;

        var user = GetUserHandlerTestData.GenerateActiveUser();
        user.Id = userId;

        var expectedResult = GetUserHandlerTestData.GenerateResultFromUser(user);

        _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<GetUserResult>(user).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Status.Should().Be(UserStatus.Active);
        result.Id.Should().Be(userId);
        await _userRepository.Received(2).GetByIdAsync(userId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that user retrieval works for inactive users.
    /// </summary>
    [Fact(DisplayName = "Given inactive user When getting user Then returns user data")]
    public async Task Handle_InactiveUser_ReturnsUserData()
    {
        // Given
        var command = GetUserHandlerTestData.GenerateValidCommand();
        var userId = command.Id;

        var user = GetUserHandlerTestData.GenerateInactiveUser();
        user.Id = userId;

        var expectedResult = GetUserHandlerTestData.GenerateResultFromUser(user);

        _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<GetUserResult>(user).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Status.Should().Be(UserStatus.Inactive);
        result.Id.Should().Be(userId);
        await _userRepository.Received(2).GetByIdAsync(userId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that user retrieval works for admin users.
    /// </summary>
    [Fact(DisplayName = "Given admin user When getting user Then returns user data")]
    public async Task Handle_AdminUser_ReturnsUserData()
    {
        // Given
        var command = GetUserHandlerTestData.GenerateValidCommand();
        var userId = command.Id;

        var user = GetUserHandlerTestData.GenerateAdminUser();
        user.Id = userId;

        var expectedResult = GetUserHandlerTestData.GenerateResultFromUser(user);

        _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<GetUserResult>(user).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Role.Should().Be(UserRole.Admin);
        result.Id.Should().Be(userId);
        await _userRepository.Received(2).GetByIdAsync(userId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that user retrieval works for customer users.
    /// </summary>
    [Fact(DisplayName = "Given customer user When getting user Then returns user data")]
    public async Task Handle_CustomerUser_ReturnsUserData()
    {
        // Given
        var command = GetUserHandlerTestData.GenerateValidCommand();
        var userId = command.Id;

        var user = GetUserHandlerTestData.GenerateCustomerUser();
        user.Id = userId;

        var expectedResult = GetUserHandlerTestData.GenerateResultFromUser(user);

        _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<GetUserResult>(user).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Role.Should().Be(UserRole.Customer);
        result.Id.Should().Be(userId);
        await _userRepository.Received(2).GetByIdAsync(userId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that logging is performed correctly.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then logs information correctly")]
    public async Task Handle_ValidRequest_LogsInformationCorrectly()
    {
        // Given
        var command = GetUserHandlerTestData.GenerateValidCommand();
        var userId = command.Id;

        var user = GetUserHandlerTestData.GenerateUserWithId(userId);
        var expectedResult = GetUserHandlerTestData.GenerateResultFromUser(user);

        _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<GetUserResult>(user).Returns(expectedResult);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _logger.Received(2).Log(
            Arg.Any<LogLevel>(),
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());
    }

    /// <summary>
    /// Tests that validation failure is logged correctly.
    /// </summary>
    [Fact(DisplayName = "Given invalid command When handling Then logs validation errors")]
    public async Task Handle_InvalidRequest_LogsValidationErrors()
    {
        // Given
        var command = GetUserHandlerTestData.GenerateInvalidCommand();

        _userRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((User?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
        _logger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());
    }

    /// <summary>
    /// Tests that user retrieval with specific ID works correctly.
    /// </summary>
    [Fact(DisplayName = "Given specific user ID When getting user Then returns correct user")]
    public async Task Handle_SpecificUserId_ReturnsCorrectUser()
    {
        // Given
        var userId = Guid.NewGuid();
        var command = GetUserHandlerTestData.GenerateCommandWithId(userId);

        var user = GetUserHandlerTestData.GenerateUserWithId(userId);
        var expectedResult = GetUserHandlerTestData.GenerateResultFromUser(user);

        _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<GetUserResult>(user).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        result.Should().BeEquivalentTo(expectedResult);
        await _userRepository.Received(2).GetByIdAsync(userId, Arg.Any<CancellationToken>());
    }
} 