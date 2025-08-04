using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsHandler : IRequestHandler<ListProductsCommand, ListProductsResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListProductsHandler> _logger;

    public ListProductsHandler(
        IProductRepository productRepository, 
        IMapper mapper,
        ILogger<ListProductsHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ListProductsResult> Handle(ListProductsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listing products with page {PageNumber} and size {PageSize}", request.PageNumber, request.PageSize);

        var (products, totalCount) = await _productRepository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

        _logger.LogInformation("Found {Count} products, total: {TotalCount}", products.Count(), totalCount);

        var productItems = _mapper.Map<IEnumerable<ProductListItem>>(products);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        _logger.LogDebug("Returning {ProductCount} products with {TotalPages} total pages", productItems.Count(), totalPages);

        return new ListProductsResult
        {
            Products = productItems,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };
    }
} 