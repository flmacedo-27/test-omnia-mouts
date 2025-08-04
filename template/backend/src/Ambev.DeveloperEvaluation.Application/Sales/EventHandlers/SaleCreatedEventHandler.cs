using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.EventHandlers;

/// <summary>
/// Handler for SaleCreatedEvent that processes sale creation events
/// </summary>
public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;

    public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Sale created event received - SaleId: {SaleId}, SaleNumber: {SaleNumber}, CustomerId: {CustomerId}, BranchId: {BranchId}, TotalAmount: {TotalAmount}, CreatedAt: {CreatedAt}",
            notification.SaleId,
            notification.SaleNumber,
            notification.CustomerId,
            notification.BranchId,
            notification.TotalAmount,
            notification.CreatedAt);

        await Task.CompletedTask;
    }
} 