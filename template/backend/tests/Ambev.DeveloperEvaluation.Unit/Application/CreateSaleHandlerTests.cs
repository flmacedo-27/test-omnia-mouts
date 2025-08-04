using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
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
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly IMediator _mediator;
    private readonly CreateSaleCommandValidator _validator;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandlerTests"/> class.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();
        _mediator = Substitute.For<IMediator>();
        _validator = new CreateSaleCommandValidator(_customerRepository, _branchRepository, _productRepository);
        _handler = new CreateSaleHandler(_saleRepository, _productRepository, _mapper, _logger, _mediator, _validator);
    }

    /// <summary>
    /// Tests that a valid sale creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var customerId = command.CustomerId;
        var branchId = command.BranchId;

        var customer = new Customer
        {
            Id = customerId,
            Name = "João Silva",
            Active = true
        };

        var branch = new Branch
        {
            Id = branchId,
            Name = "Filial Centro"
        };

        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            BranchId = branchId,
            Status = SaleStatus.Active,
            TotalAmount = 50.00m
        };

        var result = new CreateSaleResult
        {
            Id = sale.Id,
            CustomerId = sale.CustomerId,
            BranchId = sale.BranchId,
            Status = sale.Status,
            TotalAmount = sale.TotalAmount
        };

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>()).Returns(branch);
        
        foreach (var item in command.Items)
        {
            var product = new Product
            {
                Id = item.ProductId,
                Name = $"Product {item.ProductId}",
                Price = item.UnitPrice,
                StockQuantity = 100,
                Active = true
            };
            _productRepository.GetByIdAsync(item.ProductId, Arg.Any<CancellationToken>()).Returns(product);
        }
        
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        // When
        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createSaleResult.Should().NotBeNull();
        createSaleResult.Id.Should().Be(sale.Id);
        createSaleResult.CustomerId.Should().Be(customerId);
        createSaleResult.BranchId.Should().Be(branchId);
        createSaleResult.Status.Should().Be(SaleStatus.Active);
        createSaleResult.TotalAmount.Should().Be(50.00m);
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid sale creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.Empty,
            BranchId = Guid.Empty,
            Items = new List<CreateSaleItemCommand>()
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that sale is created with correct discount calculation for 4+ items.
    /// </summary>
    [Fact(DisplayName = "Given 4+ items When creating sale Then applies 10% discount")]
    public async Task Handle_FourOrMoreItems_AppliesTenPercentDiscount()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateCommandForTenPercentDiscount();
        var customerId = command.CustomerId;
        var branchId = command.BranchId;

        var customer = new Customer { Id = customerId, Active = true };
        var branch = new Branch { Id = branchId };

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>()).Returns(branch);
        
        foreach (var item in command.Items)
        {
            var product = new Product { Id = item.ProductId, StockQuantity = 100, Active = true };
            _productRepository.GetByIdAsync(item.ProductId, Arg.Any<CancellationToken>()).Returns(product);
        }
        
        var createdSale = new Sale
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            BranchId = branchId,
            Status = SaleStatus.Active,
            TotalAmount = 0
        };
        
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(createdSale);
        _mapper.Map<CreateSaleResult>(createdSale).Returns(new CreateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        var expectedTotal = 45.00m;
        
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s => s.TotalAmount == expectedTotal),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that sale is created with correct discount calculation for 10-20 items.
    /// </summary>
    [Fact(DisplayName = "Given 10-20 items When creating sale Then applies 20% discount")]
    public async Task Handle_TenToTwentyItems_AppliesTwentyPercentDiscount()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateCommandForTwentyPercentDiscount();
        var customerId = command.CustomerId;
        var branchId = command.BranchId;

        var customer = new Customer { Id = customerId, Active = true };
        var branch = new Branch { Id = branchId };

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>()).Returns(branch);
        
        foreach (var item in command.Items)
        {
            var product = new Product { Id = item.ProductId, StockQuantity = 100, Active = true };
            _productRepository.GetByIdAsync(item.ProductId, Arg.Any<CancellationToken>()).Returns(product);
        }
        
        var createdSale = new Sale
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            BranchId = branchId,
            Status = SaleStatus.Active,
            TotalAmount = 0
        };
        
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(createdSale);
        _mapper.Map<CreateSaleResult>(createdSale).Returns(new CreateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        var expectedTotal = 120.00m;
        
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s => s.TotalAmount == expectedTotal),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that sale is created with no discount for less than 4 items.
    /// </summary>
    [Fact(DisplayName = "Given less than 4 items When creating sale Then applies no discount")]
    public async Task Handle_LessThanFourItems_AppliesNoDiscount()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateCommandForNoDiscount();
        var customerId = command.CustomerId;
        var branchId = command.BranchId;

        var customer = new Customer { Id = customerId, Active = true };
        var branch = new Branch { Id = branchId };

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>()).Returns(branch);
        
        foreach (var item in command.Items)
        {
            var product = new Product { Id = item.ProductId, StockQuantity = 100, Active = true };
            _productRepository.GetByIdAsync(item.ProductId, Arg.Any<CancellationToken>()).Returns(product);
        }
        
        var createdSale = new Sale
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            BranchId = branchId,
            Status = SaleStatus.Active,
            TotalAmount = 0
        };
        
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(createdSale);
        _mapper.Map<CreateSaleResult>(createdSale).Returns(new CreateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        var expectedTotal = 20.00m;
        
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s => s.TotalAmount == expectedTotal),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that sale creation fails when customer is inactive.
    /// </summary>
    [Fact(DisplayName = "Given inactive customer When creating sale Then throws validation exception")]
    public async Task Handle_InactiveCustomer_ThrowsValidationException()
    {
        // Given
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var command = new CreateSaleCommand
        {
            CustomerId = customerId,
            BranchId = branchId,
            Items = new List<CreateSaleItemCommand>
            {
                new CreateSaleItemCommand
                {
                    ProductId = productId,
                    Quantity = 5,
                    UnitPrice = 10.00m
                }
            }
        };

        var customer = new Customer { Id = customerId, Active = false };
        var branch = new Branch { Id = branchId };
        var product = new Product { Id = productId, StockQuantity = 100, Active = true };

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>()).Returns(branch);
        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that sale creation fails when product is inactive.
    /// </summary>
    [Fact(DisplayName = "Given inactive product When creating sale Then throws validation exception")]
    public async Task Handle_InactiveProduct_ThrowsValidationException()
    {
        // Given
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var command = new CreateSaleCommand
        {
            CustomerId = customerId,
            BranchId = branchId,
            Items = new List<CreateSaleItemCommand>
            {
                new CreateSaleItemCommand
                {
                    ProductId = productId,
                    Quantity = 5,
                    UnitPrice = 10.00m
                }
            }
        };

        var customer = new Customer { Id = customerId, Active = true };
        var branch = new Branch { Id = branchId };
        var product = new Product { Id = productId, StockQuantity = 100, Active = false };

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>()).Returns(branch);
        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that sale creation fails when quantity is above 20.
    /// </summary>
    [Fact(DisplayName = "Given quantity above 20 When creating sale Then throws validation exception")]
    public async Task Handle_QuantityAboveTwenty_ThrowsValidationException()
    {
        // Given
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var command = new CreateSaleCommand
        {
            CustomerId = customerId,
            BranchId = branchId,
            Items = new List<CreateSaleItemCommand>
            {
                new CreateSaleItemCommand
                {
                    ProductId = productId,
                    Quantity = 25,
                    UnitPrice = 10.00m
                }
            }
        };

        var customer = new Customer { Id = customerId, Active = true };
        var branch = new Branch { Id = branchId };
        var product = new Product { Id = productId, StockQuantity = 100, Active = true };

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>()).Returns(branch);
        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);

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
        var command = CreateSaleHandlerTestData.GenerateCommandForTenPercentDiscount();
        var customerId = command.CustomerId;
        var branchId = command.BranchId;

        var customer = new Customer { Id = customerId, Active = true };
        var branch = new Branch { Id = branchId };

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>()).Returns(branch);
        
        foreach (var item in command.Items)
        {
            var product = new Product { Id = item.ProductId, StockQuantity = 100, Active = true };
            _productRepository.GetByIdAsync(item.ProductId, Arg.Any<CancellationToken>()).Returns(product);
        }
        
        var createdSale = new Sale
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            BranchId = branchId,
            Status = SaleStatus.Active,
            TotalAmount = 0
        };
        
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(createdSale);
        _mapper.Map<CreateSaleResult>(createdSale).Returns(new CreateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _logger.Received(2).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());
    }
} 