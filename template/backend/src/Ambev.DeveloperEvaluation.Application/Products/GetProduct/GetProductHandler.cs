using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductHandler : IRequestHandler<GetProductCommand, GetProductResult?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductHandler> _logger;
    private readonly GetProductCommandValidator _validator;

    public GetProductHandler(
        IProductRepository productRepository, 
        IMapper mapper,
        ILogger<GetProductHandler> logger,
        GetProductCommandValidator validator)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    public async Task<GetProductResult?> Handle(GetProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting product with ID: {ProductId}", request.Id);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for product retrieval: {ValidationErrors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        
        _logger.LogDebug("Product retrieved successfully with ID: {ProductId}", request.Id);
        
        return _mapper.Map<GetProductResult>(product);
    }
} 