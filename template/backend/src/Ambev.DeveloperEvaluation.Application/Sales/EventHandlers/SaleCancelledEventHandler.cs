using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.EventHandlers;

/// <summary>
/// Handler for SaleCancelledEvent that processes sale cancellation events
/// </summary>
public class SaleCancelledEventHandler : INotificationHandler<SaleCancelledEvent>
{
    private readonly ILogger<SaleCancelledEventHandler> _logger;

    public SaleCancelledEventHandler(ILogger<SaleCancelledEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Sale cancelled event received - SaleId: {SaleId}, SaleNumber: {SaleNumber}, CancelledAt: {CancelledAt}, CancellationReason: {CancellationReason}",
            notification.SaleId,
            notification.SaleNumber,
            notification.CancelledAt,
            notification.CancellationReason);

        await Task.CompletedTask;
    }
} 