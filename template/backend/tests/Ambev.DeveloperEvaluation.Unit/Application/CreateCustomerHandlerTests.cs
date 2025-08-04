using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateCustomerHandler"/> class.
/// </summary>
public class CreateCustomerHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCustomerHandler> _logger;
    private readonly CreateCustomerCommandValidator _validator;
    private readonly CreateCustomerHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCustomerHandlerTests"/> class.
    /// </summary>
    public CreateCustomerHandlerTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateCustomerHandler>>();
        _validator = new CreateCustomerCommandValidator(_customerRepository);
        _handler = new CreateCustomerHandler(_customerRepository, _mapper, _logger, _validator);
    }

    /// <summary>
    /// Tests that a valid customer creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid customer data When creating customer Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateCustomerHandlerTestData.GenerateValidCommand();

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Email = command.Email,
            Phone = command.Phone,
            CustomerType = command.CustomerType,
            DocumentNumber = command.DocumentNumber,
            Active = true
        };

        var result = new CreateCustomerResult
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            CustomerType = customer.CustomerType,
            DocumentNumber = customer.DocumentNumber,
            Active = customer.Active
        };

        _mapper.Map<CreateCustomerResult>(customer).Returns(result);
        _customerRepository.CreateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
            .Returns(customer);

        _customerRepository.ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(false);
        _customerRepository.ExistsByDocumentNumberAsync(command.DocumentNumber, Arg.Any<CancellationToken>())
            .Returns(false);

        // When
        var createCustomerResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createCustomerResult.Should().NotBeNull();
        createCustomerResult.Id.Should().Be(customer.Id);
        createCustomerResult.Name.Should().Be(command.Name);
        createCustomerResult.Email.Should().Be(command.Email);
        createCustomerResult.Phone.Should().Be(command.Phone);
        createCustomerResult.CustomerType.Should().Be(command.CustomerType);
        createCustomerResult.DocumentNumber.Should().Be(command.DocumentNumber);
        createCustomerResult.Active.Should().BeTrue();
        await _customerRepository.Received(1).CreateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid customer creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid customer data When creating customer Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = CreateCustomerHandlerTestData.GenerateCommandWithExistingEmail();
        
        _customerRepository.ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(true);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that the customer is created with correct properties.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then creates customer with correct properties")]
    public async Task Handle_ValidRequest_CreatesCustomerWithCorrectProperties()
    {
        // Given
        var command = CreateCustomerHandlerTestData.GenerateValidCommand();

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Email = command.Email,
            Phone = command.Phone,
            CustomerType = command.CustomerType,
            DocumentNumber = command.DocumentNumber,
            Active = true
        };

        _mapper.Map<CreateCustomerResult>(customer).Returns(new CreateCustomerResult());
        _customerRepository.CreateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
            .Returns(customer);

        _customerRepository.ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(false);
        _customerRepository.ExistsByDocumentNumberAsync(command.DocumentNumber, Arg.Any<CancellationToken>())
            .Returns(false);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _customerRepository.Received(1).CreateAsync(
            Arg.Is<Customer>(c => 
                c.Name == command.Name &&
                c.Email == command.Email &&
                c.Phone == command.Phone &&
                c.CustomerType == command.CustomerType &&
                c.DocumentNumber == command.DocumentNumber &&
                c.Active == true),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that logging is performed correctly.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then logs information correctly")]
    public async Task Handle_ValidRequest_LogsInformationCorrectly()
    {
        // Given
        var command = CreateCustomerHandlerTestData.GenerateValidCommand();

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Email = command.Email,
            Phone = command.Phone,
            CustomerType = command.CustomerType,
            DocumentNumber = command.DocumentNumber,
            Active = true
        };

        _mapper.Map<CreateCustomerResult>(customer).Returns(new CreateCustomerResult());
        _customerRepository.CreateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
            .Returns(customer);

        _customerRepository.ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(false);
        _customerRepository.ExistsByDocumentNumberAsync(command.DocumentNumber, Arg.Any<CancellationToken>())
            .Returns(false);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString().Contains(command.Name)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());
    }

    /// <summary>
    /// Tests that the mapper is called with the correct customer.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps customer to result")]
    public async Task Handle_ValidRequest_MapsCustomerToResult()
    {
        // Given
        var command = CreateCustomerHandlerTestData.GenerateValidCommand();

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Email = command.Email,
            Phone = command.Phone,
            CustomerType = command.CustomerType,
            DocumentNumber = command.DocumentNumber,
            Active = true
        };

        _mapper.Map<CreateCustomerResult>(customer).Returns(new CreateCustomerResult());
        _customerRepository.CreateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
            .Returns(customer);

        _customerRepository.ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(false);
        _customerRepository.ExistsByDocumentNumberAsync(command.DocumentNumber, Arg.Any<CancellationToken>())
            .Returns(false);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<CreateCustomerResult>(customer);
    }
} 