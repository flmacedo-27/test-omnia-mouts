using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductHandler : BaseHandler<GetProductCommand, GetProductResult?, GetProductCommandValidator>
{
    private readonly IProductRepository _productRepository;

    public GetProductHandler(
        IProductRepository productRepository, 
        IMapper mapper,
        ILogger<GetProductHandler> logger,
        GetProductCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _productRepository = productRepository;
    }

    protected override async Task<GetProductResult?> ExecuteAsync(GetProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        
        return Mapper.Map<GetProductResult>(product);
    }

    protected override void LogOperationStart(GetProductCommand request)
    {
        Logger.LogInformation("Getting product with ID: {ProductId}", request.Id);
    }

    protected override void LogOperationSuccess(GetProductCommand request, GetProductResult? result)
    {
        Logger.LogInformation("Product retrieved successfully with ID: {ProductId}", request.Id);
    }
} 