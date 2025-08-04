using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
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
/// Contains unit tests for the <see cref="GetSaleHandler"/> class.
/// </summary>
public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSaleHandler> _logger;
    private readonly GetSaleCommandValidator _validator;
    private readonly GetSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleHandlerTests"/> class.
    /// </summary>
    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetSaleHandler>>();
        _validator = new GetSaleCommandValidator(_saleRepository);
        _handler = new GetSaleHandler(_saleRepository, _mapper, _logger, _validator);
    }

    /// <summary>
    /// Tests that a valid sale retrieval request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale ID When getting sale Then returns sale data")]
    public async Task Handle_ValidRequest_ReturnsSaleData()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidCommand();
        var saleId = command.Id;

        var sale = GetSaleHandlerTestData.GenerateSaleWithId(saleId);
        var expectedResult = GetSaleHandlerTestData.GenerateResultFromSale(sale);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _saleRepository.Received(2).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetSaleResult>(sale);
    }

    /// <summary>
    /// Tests that an invalid sale retrieval request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale ID When getting sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateInvalidCommand();

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Sale?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that sale retrieval works for active sales.
    /// </summary>
    [Fact(DisplayName = "Given active sale When getting sale Then returns sale data")]
    public async Task Handle_ActiveSale_ReturnsSaleData()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidCommand();
        var saleId = command.Id;

        var sale = GetSaleHandlerTestData.GenerateActiveSale();
        sale.Id = saleId;

        var expectedResult = GetSaleHandlerTestData.GenerateResultFromSale(sale);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Status.Should().Be(SaleStatus.Active);
        result.Id.Should().Be(saleId);
        await _saleRepository.Received(2).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that sale retrieval works for cancelled sales.
    /// </summary>
    [Fact(DisplayName = "Given cancelled sale When getting sale Then returns sale data")]
    public async Task Handle_CancelledSale_ReturnsSaleData()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidCommand();
        var saleId = command.Id;

        var sale = GetSaleHandlerTestData.GenerateCancelledSale();
        sale.Id = saleId;

        var expectedResult = GetSaleHandlerTestData.GenerateResultFromSale(sale);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Status.Should().Be(SaleStatus.Cancelled);
        result.Id.Should().Be(saleId);
        await _saleRepository.Received(2).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that sale retrieval works for high value sales.
    /// </summary>
    [Fact(DisplayName = "Given high value sale When getting sale Then returns sale data")]
    public async Task Handle_HighValueSale_ReturnsSaleData()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidCommand();
        var saleId = command.Id;

        var sale = GetSaleHandlerTestData.GenerateHighValueSale();
        sale.Id = saleId;

        var expectedResult = GetSaleHandlerTestData.GenerateResultFromSale(sale);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.TotalAmount.Should().BeGreaterThanOrEqualTo(500.00m);
        result.Id.Should().Be(saleId);
        await _saleRepository.Received(2).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that sale retrieval works for low value sales.
    /// </summary>
    [Fact(DisplayName = "Given low value sale When getting sale Then returns sale data")]
    public async Task Handle_LowValueSale_ReturnsSaleData()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidCommand();
        var saleId = command.Id;

        var sale = GetSaleHandlerTestData.GenerateLowValueSale();
        sale.Id = saleId;

        var expectedResult = GetSaleHandlerTestData.GenerateResultFromSale(sale);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.TotalAmount.Should().BeLessThanOrEqualTo(100.00m);
        result.Id.Should().Be(saleId);
        await _saleRepository.Received(2).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that logging is performed correctly.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then logs information correctly")]
    public async Task Handle_ValidRequest_LogsInformationCorrectly()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidCommand();
        var saleId = command.Id;

        var sale = GetSaleHandlerTestData.GenerateSaleWithId(saleId);
        var expectedResult = GetSaleHandlerTestData.GenerateResultFromSale(sale);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

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
        var command = GetSaleHandlerTestData.GenerateInvalidCommand();

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Sale?)null);

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
    /// Tests that sale retrieval with specific ID works correctly.
    /// </summary>
    [Fact(DisplayName = "Given specific sale ID When getting sale Then returns correct sale")]
    public async Task Handle_SpecificSaleId_ReturnsCorrectSale()
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = GetSaleHandlerTestData.GenerateCommandWithId(saleId);

        var sale = GetSaleHandlerTestData.GenerateSaleWithId(saleId);
        var expectedResult = GetSaleHandlerTestData.GenerateResultFromSale(sale);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Id.Should().Be(saleId);
        result.Should().BeEquivalentTo(expectedResult);
        await _saleRepository.Received(2).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
    }
} 