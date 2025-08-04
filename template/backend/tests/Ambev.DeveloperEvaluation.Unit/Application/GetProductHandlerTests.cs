using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
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
/// Contains unit tests for the <see cref="GetProductHandler"/> class.
/// </summary>
public class GetProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductHandler> _logger;
    private readonly GetProductCommandValidator _validator;
    private readonly GetProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductHandlerTests"/> class.
    /// </summary>
    public GetProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetProductHandler>>();
        _validator = new GetProductCommandValidator(_productRepository);
        _handler = new GetProductHandler(_productRepository, _mapper, _logger, _validator);
    }

    /// <summary>
    /// Tests that a valid product retrieval request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid product ID When getting product Then returns product data")]
    public async Task Handle_ValidRequest_ReturnsProductData()
    {
        // Given
        var command = GetProductHandlerTestData.GenerateValidCommand();
        var productId = command.Id;

        var product = GetProductHandlerTestData.GenerateProductWithId(productId);
        var expectedResult = GetProductHandlerTestData.GenerateResultFromProduct(product);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _productRepository.Received(2).GetByIdAsync(productId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetProductResult>(product);
    }

    /// <summary>
    /// Tests that an invalid product retrieval request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid product ID When getting product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = GetProductHandlerTestData.GenerateInvalidCommand();

        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Product?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that product retrieval works for active products.
    /// </summary>
    [Fact(DisplayName = "Given active product When getting product Then returns product data")]
    public async Task Handle_ActiveProduct_ReturnsProductData()
    {
        // Given
        var command = GetProductHandlerTestData.GenerateValidCommand();
        var productId = command.Id;

        var product = GetProductHandlerTestData.GenerateActiveProduct();
        product.Id = productId;

        var expectedResult = GetProductHandlerTestData.GenerateResultFromProduct(product);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Active.Should().BeTrue();
        result.Id.Should().Be(productId);
        await _productRepository.Received(2).GetByIdAsync(productId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that product retrieval works for inactive products.
    /// </summary>
    [Fact(DisplayName = "Given inactive product When getting product Then returns product data")]
    public async Task Handle_InactiveProduct_ReturnsProductData()
    {
        // Given
        var command = GetProductHandlerTestData.GenerateValidCommand();
        var productId = command.Id;

        var product = GetProductHandlerTestData.GenerateInactiveProduct();
        product.Id = productId;

        var expectedResult = GetProductHandlerTestData.GenerateResultFromProduct(product);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Active.Should().BeFalse();
        result.Id.Should().Be(productId);
        await _productRepository.Received(2).GetByIdAsync(productId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that product retrieval works for high price products.
    /// </summary>
    [Fact(DisplayName = "Given high price product When getting product Then returns product data")]
    public async Task Handle_HighPriceProduct_ReturnsProductData()
    {
        // Given
        var command = GetProductHandlerTestData.GenerateValidCommand();
        var productId = command.Id;

        var product = GetProductHandlerTestData.GenerateHighPriceProduct();
        product.Id = productId;

        var expectedResult = GetProductHandlerTestData.GenerateResultFromProduct(product);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Price.Should().BeGreaterThanOrEqualTo(500.00m);
        result.Id.Should().Be(productId);
        await _productRepository.Received(2).GetByIdAsync(productId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that product retrieval works for low price products.
    /// </summary>
    [Fact(DisplayName = "Given low price product When getting product Then returns product data")]
    public async Task Handle_LowPriceProduct_ReturnsProductData()
    {
        // Given
        var command = GetProductHandlerTestData.GenerateValidCommand();
        var productId = command.Id;

        var product = GetProductHandlerTestData.GenerateLowPriceProduct();
        product.Id = productId;

        var expectedResult = GetProductHandlerTestData.GenerateResultFromProduct(product);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Price.Should().BeLessThanOrEqualTo(10.00m);
        result.Id.Should().Be(productId);
        await _productRepository.Received(2).GetByIdAsync(productId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that product retrieval works for high stock products.
    /// </summary>
    [Fact(DisplayName = "Given high stock product When getting product Then returns product data")]
    public async Task Handle_HighStockProduct_ReturnsProductData()
    {
        // Given
        var command = GetProductHandlerTestData.GenerateValidCommand();
        var productId = command.Id;

        var product = GetProductHandlerTestData.GenerateHighStockProduct();
        product.Id = productId;

        var expectedResult = GetProductHandlerTestData.GenerateResultFromProduct(product);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.StockQuantity.Should().BeGreaterThanOrEqualTo(500);
        result.Id.Should().Be(productId);
        await _productRepository.Received(2).GetByIdAsync(productId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that product retrieval works for zero stock products.
    /// </summary>
    [Fact(DisplayName = "Given zero stock product When getting product Then returns product data")]
    public async Task Handle_ZeroStockProduct_ReturnsProductData()
    {
        // Given
        var command = GetProductHandlerTestData.GenerateValidCommand();
        var productId = command.Id;

        var product = GetProductHandlerTestData.GenerateZeroStockProduct();
        product.Id = productId;

        var expectedResult = GetProductHandlerTestData.GenerateResultFromProduct(product);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.StockQuantity.Should().Be(0);
        result.Id.Should().Be(productId);
        await _productRepository.Received(2).GetByIdAsync(productId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that logging is performed correctly.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then logs information correctly")]
    public async Task Handle_ValidRequest_LogsInformationCorrectly()
    {
        // Given
        var command = GetProductHandlerTestData.GenerateValidCommand();
        var productId = command.Id;

        var product = GetProductHandlerTestData.GenerateProductWithId(productId);
        var expectedResult = GetProductHandlerTestData.GenerateResultFromProduct(product);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(expectedResult);

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
        var command = GetProductHandlerTestData.GenerateInvalidCommand();

        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Product?)null);

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
    /// Tests that product retrieval with specific ID works correctly.
    /// </summary>
    [Fact(DisplayName = "Given specific product ID When getting product Then returns correct product")]
    public async Task Handle_SpecificProductId_ReturnsCorrectProduct()
    {
        // Given
        var productId = Guid.NewGuid();
        var command = GetProductHandlerTestData.GenerateCommandWithId(productId);

        var product = GetProductHandlerTestData.GenerateProductWithId(productId);
        var expectedResult = GetProductHandlerTestData.GenerateResultFromProduct(product);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result!.Id.Should().Be(productId);
        result.Should().BeEquivalentTo(expectedResult);
        await _productRepository.Received(2).GetByIdAsync(productId, Arg.Any<CancellationToken>());
    }
} 