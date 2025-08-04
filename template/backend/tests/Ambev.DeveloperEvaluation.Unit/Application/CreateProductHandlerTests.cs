using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
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
/// Contains unit tests for the <see cref="CreateProductHandler"/> class.
/// </summary>
public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductHandler> _logger;
    private readonly CreateProductCommandValidator _validator;
    private readonly CreateProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductHandlerTests"/> class.
    /// </summary>
    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateProductHandler>>();
        _validator = new CreateProductCommandValidator(_productRepository);
        _handler = new CreateProductHandler(_productRepository, _mapper, _logger, _validator);
    }

    /// <summary>
    /// Tests that a valid product creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid product data When creating product Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateValidCommand();
        var createdProduct = CreateProductHandlerTestData.GenerateValidProduct();
        var expectedResult = CreateProductHandlerTestData.GenerateResultFromProduct(createdProduct);

        _productRepository.ExistsByCodeAsync(command.Code, Arg.Any<CancellationToken>()).Returns(false);
        _productRepository.ExistsBySKUAsync(command.SKU, Arg.Any<CancellationToken>()).Returns(false);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()).Returns(createdProduct);
        _mapper.Map<CreateProductResult>(createdProduct).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<CreateProductResult>(createdProduct);
    }

    /// <summary>
    /// Tests that product creation works with high price products.
    /// </summary>
    [Fact(DisplayName = "Given high price product When creating product Then returns success response")]
    public async Task Handle_HighPriceProduct_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateCommandWithHighPrice();
        var createdProduct = CreateProductHandlerTestData.GenerateValidProduct();
        var expectedResult = CreateProductHandlerTestData.GenerateResultFromProduct(createdProduct);

        _productRepository.ExistsByCodeAsync(command.Code, Arg.Any<CancellationToken>()).Returns(false);
        _productRepository.ExistsBySKUAsync(command.SKU, Arg.Any<CancellationToken>()).Returns(false);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()).Returns(createdProduct);
        _mapper.Map<CreateProductResult>(createdProduct).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that product creation works with low price products.
    /// </summary>
    [Fact(DisplayName = "Given low price product When creating product Then returns success response")]
    public async Task Handle_LowPriceProduct_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateCommandWithLowPrice();
        var createdProduct = CreateProductHandlerTestData.GenerateValidProduct();
        var expectedResult = CreateProductHandlerTestData.GenerateResultFromProduct(createdProduct);

        _productRepository.ExistsByCodeAsync(command.Code, Arg.Any<CancellationToken>()).Returns(false);
        _productRepository.ExistsBySKUAsync(command.SKU, Arg.Any<CancellationToken>()).Returns(false);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()).Returns(createdProduct);
        _mapper.Map<CreateProductResult>(createdProduct).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that product creation works with high stock products.
    /// </summary>
    [Fact(DisplayName = "Given high stock product When creating product Then returns success response")]
    public async Task Handle_HighStockProduct_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateCommandWithHighStock();
        var createdProduct = CreateProductHandlerTestData.GenerateValidProduct();
        var expectedResult = CreateProductHandlerTestData.GenerateResultFromProduct(createdProduct);

        _productRepository.ExistsByCodeAsync(command.Code, Arg.Any<CancellationToken>()).Returns(false);
        _productRepository.ExistsBySKUAsync(command.SKU, Arg.Any<CancellationToken>()).Returns(false);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()).Returns(createdProduct);
        _mapper.Map<CreateProductResult>(createdProduct).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that product creation works with zero stock products.
    /// </summary>
    [Fact(DisplayName = "Given zero stock product When creating product Then returns success response")]
    public async Task Handle_ZeroStockProduct_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateCommandWithZeroStock();
        var createdProduct = CreateProductHandlerTestData.GenerateValidProduct();
        var expectedResult = CreateProductHandlerTestData.GenerateResultFromProduct(createdProduct);

        _productRepository.ExistsByCodeAsync(command.Code, Arg.Any<CancellationToken>()).Returns(false);
        _productRepository.ExistsBySKUAsync(command.SKU, Arg.Any<CancellationToken>()).Returns(false);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()).Returns(createdProduct);
        _mapper.Map<CreateProductResult>(createdProduct).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that product creation with existing SKU throws validation exception.
    /// </summary>
    [Fact(DisplayName = "Given existing SKU When creating product Then throws validation exception")]
    public async Task Handle_ExistingSKU_ThrowsValidationException()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateCommandWithExistingSKU();

        _productRepository.ExistsBySKUAsync(command.SKU, Arg.Any<CancellationToken>()).Returns(true);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that product creation with existing code throws validation exception.
    /// </summary>
    [Fact(DisplayName = "Given existing code When creating product Then throws validation exception")]
    public async Task Handle_ExistingCode_ThrowsValidationException()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateCommandWithExistingCode();

        _productRepository.ExistsByCodeAsync(command.Code, Arg.Any<CancellationToken>()).Returns(true);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that logging is performed correctly.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then logs information correctly")]
    public async Task Handle_ValidRequest_LogsInformationCorrectly()
    {
        // Given
        var command = CreateProductHandlerTestData.GenerateValidCommand();
        var createdProduct = CreateProductHandlerTestData.GenerateValidProduct();
        var expectedResult = CreateProductHandlerTestData.GenerateResultFromProduct(createdProduct);

        _productRepository.ExistsByCodeAsync(command.Code, Arg.Any<CancellationToken>()).Returns(false);
        _productRepository.ExistsBySKUAsync(command.SKU, Arg.Any<CancellationToken>()).Returns(false);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()).Returns(createdProduct);
        _mapper.Map<CreateProductResult>(createdProduct).Returns(expectedResult);

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
    /// Tests that product creation with specific data works correctly.
    /// </summary>
    [Fact(DisplayName = "Given specific product data When creating product Then returns correct product")]
    public async Task Handle_SpecificProductData_ReturnsCorrectProduct()
    {
        // Given
        var command = new CreateProductCommand
        {
            Name = "Test Product",
            Code = "TEST001",
            Description = "Test Description",
            Price = 99.99m,
            StockQuantity = 50,
            SKU = "TEST-SKU-001"
        };

        var createdProduct = CreateProductHandlerTestData.GenerateValidProduct();
        var expectedResult = CreateProductHandlerTestData.GenerateResultFromProduct(createdProduct);

        _productRepository.ExistsByCodeAsync(command.Code, Arg.Any<CancellationToken>()).Returns(false);
        _productRepository.ExistsBySKUAsync(command.SKU, Arg.Any<CancellationToken>()).Returns(false);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()).Returns(createdProduct);
        _mapper.Map<CreateProductResult>(createdProduct).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }
} 