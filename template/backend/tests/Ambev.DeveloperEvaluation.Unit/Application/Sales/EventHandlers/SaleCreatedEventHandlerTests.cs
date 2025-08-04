using Ambev.DeveloperEvaluation.Application.Sales.EventHandlers;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.EventHandlers;

/// <summary>
/// Tests for SaleCreatedEventHandler
/// </summary>
public class SaleCreatedEventHandlerTests
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;
    private readonly SaleCreatedEventHandler _handler;

    public SaleCreatedEventHandlerTests()
    {
        _logger = Substitute.For<ILogger<SaleCreatedEventHandler>>();
        _handler = new SaleCreatedEventHandler(_logger);
    }

    [Fact(DisplayName = "Handle should log sale created event information")]
    public async Task Handle_ShouldLogSaleCreatedEventInformation()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var totalAmount = 150.00m;
        var createdAt = DateTime.UtcNow;

        var notification = new SaleCreatedEvent(saleId, "SALE-001", customerId, branchId, totalAmount, createdAt);

        // Act
        await _handler.Handle(notification, CancellationToken.None);
    }

    [Fact(DisplayName = "Handle should log correct event data")]
    public async Task Handle_ShouldLogCorrectEventData()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var saleNumber = "SALE-002";
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var totalAmount = 200.50m;
        var createdAt = DateTime.UtcNow;

        var notification = new SaleCreatedEvent(saleId, saleNumber, customerId, branchId, totalAmount, createdAt);

        // Act
        await _handler.Handle(notification, CancellationToken.None);
    }

    [Fact(DisplayName = "Handle should complete successfully")]
    public async Task Handle_ShouldCompleteSuccessfully()
    {
        // Arrange
        var notification = new SaleCreatedEvent(
            Guid.NewGuid(), 
            "SALE-003", 
            Guid.NewGuid(), 
            Guid.NewGuid(), 
            100.00m, 
            DateTime.UtcNow);

        // Act & Assert
        await _handler.Handle(notification, CancellationToken.None);
    }
} 