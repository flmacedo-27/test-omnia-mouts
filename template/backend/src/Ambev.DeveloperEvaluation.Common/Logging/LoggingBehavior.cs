using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Ambev.DeveloperEvaluation.Common.Logging;

/// <summary>
/// MediatR behavior that adds structured logging for all requests
/// </summary>
/// <typeparam name="TRequest">The request type</typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly Stopwatch _stopwatch;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
        _stopwatch = new Stopwatch();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var requestId = Guid.NewGuid();

        _logger.LogInformation(
            "Handling {RequestName} with ID {RequestId}",
            requestName,
            requestId);

        _stopwatch.Start();

        try
        {
            var response = await next();

            _stopwatch.Stop();

            _logger.LogInformation(
                "Handled {RequestName} with ID {RequestId} successfully in {ElapsedMilliseconds}ms",
                requestName,
                requestId,
                _stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();

            _logger.LogError(
                ex,
                "Error handling {RequestName} with ID {RequestId} after {ElapsedMilliseconds}ms",
                requestName,
                requestId,
                _stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
} 