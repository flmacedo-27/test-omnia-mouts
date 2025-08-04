using Ambev.DeveloperEvaluation.Application.Sales.EventHandlers;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.EventHandlers;

/// <summary>
/// Tests for SaleCancelledEventHandler
/// </summary>
public class SaleCancelledEventHandlerTests
{
    private readonly ILogger<SaleCancelledEventHandler> _logger;
    private readonly SaleCancelledEventHandler _handler;

    public SaleCancelledEventHandlerTests()
    {
        _logger = Substitute.For<ILogger<SaleCancelledEventHandler>>();
        _handler = new SaleCancelledEventHandler(_logger);
    }

    [Fact(DisplayName = "Handle should log sale cancelled event information")]
    public async Task Handle_ShouldLogSaleCancelledEventInformation()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var saleNumber = "SALE-001";
        var cancelledAt = DateTime.UtcNow;
        var cancellationReason = "Customer request";

        var notification = new SaleCancelledEvent(saleId, saleNumber, cancellationReason, cancelledAt);

        // Act
        await _handler.Handle(notification, CancellationToken.None);
    }

    [Fact(DisplayName = "Handle should log correct cancellation data")]
    public async Task Handle_ShouldLogCorrectCancellationData()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var saleNumber = "SALE-002";
        var cancelledAt = DateTime.UtcNow;
        var cancellationReason = "Out of stock";

        var notification = new SaleCancelledEvent(saleId, saleNumber, cancellationReason, cancelledAt);

        // Act
        await _handler.Handle(notification, CancellationToken.None);
    }

    [Fact(DisplayName = "Handle should complete successfully")]
    public async Task Handle_ShouldCompleteSuccessfully()
    {
        // Arrange
        var notification = new SaleCancelledEvent(
            Guid.NewGuid(), 
            "SALE-003",
            "Test cancellation",
            DateTime.UtcNow);

        // Act & Assert
        await _handler.Handle(notification, CancellationToken.None);
    }
} 