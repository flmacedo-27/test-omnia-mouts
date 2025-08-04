using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.Domain.Entities;
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
/// Contains unit tests for the <see cref="GetCustomerHandler"/> class.
/// </summary>
public class GetCustomerHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCustomerHandler> _logger;
    private readonly GetCustomerCommandValidator _validator;
    private readonly GetCustomerHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCustomerHandlerTests"/> class.
    /// </summary>
    public GetCustomerHandlerTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetCustomerHandler>>();
        _validator = new GetCustomerCommandValidator(_customerRepository);
        _handler = new GetCustomerHandler(_customerRepository, _mapper, _logger, _validator);
    }

    /// <summary>
    /// Tests that a valid customer retrieval request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid customer ID When getting customer Then returns customer data")]
    public async Task Handle_ValidRequest_ReturnsCustomerData()
    {
        // Given
        var command = GetCustomerHandlerTestData.GenerateValidCommand();
        var customerId = command.Id;

        var customer = GetCustomerHandlerTestData.GenerateCustomerWithId(customerId);
        var expectedResult = GetCustomerHandlerTestData.GenerateValidResult();

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _mapper.Map<GetCustomerResult>(customer).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _customerRepository.Received(2).GetByIdAsync(customerId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetCustomerResult>(customer);
    }

    /// <summary>
    /// Tests that an invalid customer retrieval request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid customer ID When getting customer Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = GetCustomerHandlerTestData.GenerateInvalidCommand();

        _customerRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Customer?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that customer retrieval returns null when customer is not found.
    /// </summary>
    [Fact(DisplayName = "Given non-existent customer ID When getting customer Then returns null")]
    public async Task Handle_NonExistentCustomer_ReturnsNull()
    {
        // Given
        var command = GetCustomerHandlerTestData.GenerateValidCommand();
        var customerId = command.Id;

        var customer = GetCustomerHandlerTestData.GenerateCustomerWithId(customerId);

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _mapper.Map<GetCustomerResult>(customer).Returns((GetCustomerResult?)null);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().BeNull();
        await _customerRepository.Received(2).GetByIdAsync(customerId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetCustomerResult>(customer);
    }

    /// <summary>
    /// Tests that customer retrieval works for active customers.
    /// </summary>
    [Fact(DisplayName = "Given active customer When getting customer Then returns customer data")]
    public async Task Handle_ActiveCustomer_ReturnsCustomerData()
    {
        // Given
        var command = GetCustomerHandlerTestData.GenerateValidCommand();
        var customerId = command.Id;

        var customer = GetCustomerHandlerTestData.GenerateActiveCustomer();
        customer.Id = customerId;

        var expectedResult = new GetCustomerResult
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            CustomerType = customer.CustomerType,
            DocumentNumber = customer.DocumentNumber,
            Active = customer.Active,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _mapper.Map<GetCustomerResult>(customer).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Active.Should().BeTrue();
        result.Id.Should().Be(customerId);
        await _customerRepository.Received(2).GetByIdAsync(customerId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that customer retrieval works for inactive customers.
    /// </summary>
    [Fact(DisplayName = "Given inactive customer When getting customer Then returns customer data")]
    public async Task Handle_InactiveCustomer_ReturnsCustomerData()
    {
        // Given
        var command = GetCustomerHandlerTestData.GenerateValidCommand();
        var customerId = command.Id;

        var customer = GetCustomerHandlerTestData.GenerateInactiveCustomer();
        customer.Id = customerId;

        var expectedResult = new GetCustomerResult
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            CustomerType = customer.CustomerType,
            DocumentNumber = customer.DocumentNumber,
            Active = customer.Active,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _mapper.Map<GetCustomerResult>(customer).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Active.Should().BeFalse();
        result.Id.Should().Be(customerId);
        await _customerRepository.Received(2).GetByIdAsync(customerId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that customer retrieval works for individual customers.
    /// </summary>
    [Fact(DisplayName = "Given individual customer When getting customer Then returns customer data")]
    public async Task Handle_IndividualCustomer_ReturnsCustomerData()
    {
        // Given
        var command = GetCustomerHandlerTestData.GenerateValidCommand();
        var customerId = command.Id;

        var customer = GetCustomerHandlerTestData.GenerateIndividualCustomer();
        customer.Id = customerId;

        var expectedResult = new GetCustomerResult
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            CustomerType = customer.CustomerType,
            DocumentNumber = customer.DocumentNumber,
            Active = customer.Active,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _mapper.Map<GetCustomerResult>(customer).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.CustomerType.Should().Be(customer.CustomerType);
        result.Id.Should().Be(customerId);
        await _customerRepository.Received(2).GetByIdAsync(customerId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that customer retrieval works for corporate customers.
    /// </summary>
    [Fact(DisplayName = "Given corporate customer When getting customer Then returns customer data")]
    public async Task Handle_CorporateCustomer_ReturnsCustomerData()
    {
        // Given
        var command = GetCustomerHandlerTestData.GenerateValidCommand();
        var customerId = command.Id;

        var customer = GetCustomerHandlerTestData.GenerateCorporateCustomer();
        customer.Id = customerId;

        var expectedResult = new GetCustomerResult
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            CustomerType = customer.CustomerType,
            DocumentNumber = customer.DocumentNumber,
            Active = customer.Active,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _mapper.Map<GetCustomerResult>(customer).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.CustomerType.Should().Be(customer.CustomerType);
        result.Id.Should().Be(customerId);
        await _customerRepository.Received(2).GetByIdAsync(customerId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that logging is performed correctly.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then logs information correctly")]
    public async Task Handle_ValidRequest_LogsInformationCorrectly()
    {
        // Given
        var command = GetCustomerHandlerTestData.GenerateValidCommand();
        var customerId = command.Id;

        var customer = GetCustomerHandlerTestData.GenerateCustomerWithId(customerId);
        var expectedResult = GetCustomerHandlerTestData.GenerateValidResult();

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _mapper.Map<GetCustomerResult>(customer).Returns(expectedResult);

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
        var command = GetCustomerHandlerTestData.GenerateInvalidCommand();

        _customerRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Customer?)null);

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
    /// Tests that customer retrieval with specific ID works correctly.
    /// </summary>
    [Fact(DisplayName = "Given specific customer ID When getting customer Then returns correct customer")]
    public async Task Handle_SpecificCustomerId_ReturnsCorrectCustomer()
    {
        // Given
        var customerId = Guid.NewGuid();
        var command = GetCustomerHandlerTestData.GenerateCommandWithId(customerId);

        var customer = GetCustomerHandlerTestData.GenerateCustomerWithId(customerId);
        var expectedResult = new GetCustomerResult
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            CustomerType = customer.CustomerType,
            DocumentNumber = customer.DocumentNumber,
            Active = customer.Active,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _mapper.Map<GetCustomerResult>(customer).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Id.Should().Be(customerId);
        result.Should().BeEquivalentTo(expectedResult);
        await _customerRepository.Received(2).GetByIdAsync(customerId, Arg.Any<CancellationToken>());
    }
} 