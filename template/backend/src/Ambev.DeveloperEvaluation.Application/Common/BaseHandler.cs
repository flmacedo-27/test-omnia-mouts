using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Common;

/// <summary>
/// Base class for handlers that provides common functionality for validation and logging
/// </summary>
/// <typeparam name="TRequest">The request type</typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
/// <typeparam name="TValidator">The validator type</typeparam>
public abstract class BaseHandler<TRequest, TResponse, TValidator> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TValidator : IValidator<TRequest>
{
    protected readonly IMapper Mapper;
    protected readonly ILogger Logger;
    protected readonly TValidator Validator;

    protected BaseHandler(IMapper mapper, ILogger logger, TValidator validator)
    {
        Mapper = mapper;
        Logger = logger;
        Validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        // Log operation start
        LogOperationStart(request);

        // Validate request
        await ValidateRequest(request, cancellationToken);

        // Execute business logic
        var result = await ExecuteAsync(request, cancellationToken);

        // Log operation success
        LogOperationSuccess(request, result);

        return result;
    }

    /// <summary>
    /// Executes the business logic for the handler
    /// </summary>
    /// <param name="request">The request to handle</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The response</returns>
    protected abstract Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Logs the start of the operation
    /// </summary>
    /// <param name="request">The request being processed</param>
    protected virtual void LogOperationStart(TRequest request)
    {
        Logger.LogInformation("Starting operation for {RequestType}", typeof(TRequest).Name);
    }

    /// <summary>
    /// Logs the successful completion of the operation
    /// </summary>
    /// <param name="request">The original request</param>
    /// <param name="result">The result of the operation</param>
    protected virtual void LogOperationSuccess(TRequest request, TResponse result)
    {
        Logger.LogInformation("Operation completed successfully for {RequestType}", typeof(TRequest).Name);
    }

    /// <summary>
    /// Validates the request using the provided validator
    /// </summary>
    /// <param name="request">The request to validate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    private async Task ValidateRequest(TRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await Validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            Logger.LogWarning("Validation failed for {RequestType}: {ValidationErrors}", 
                typeof(TRequest).Name,
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            
            throw new ValidationException(validationResult.Errors);
        }
    }
} 