using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsHandler : BaseHandler<ListProductsCommand, ListProductsResult, ListProductsCommandValidator>
{
    private readonly IProductRepository _productRepository;

    public ListProductsHandler(
        IProductRepository productRepository, 
        IMapper mapper,
        ILogger<ListProductsHandler> logger,
        ListProductsCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _productRepository = productRepository;
    }

    protected override async Task<ListProductsResult> ExecuteAsync(ListProductsCommand request, CancellationToken cancellationToken)
    {
        var (products, totalCount) = await _productRepository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

        var productItems = Mapper.Map<IEnumerable<ProductListItem>>(products);

        return new ListProductsResult(productItems, totalCount, request.PageNumber, request.PageSize);
    }

    protected override void LogOperationStart(ListProductsCommand request)
    {
        Logger.LogInformation("Listing products with page {PageNumber} and size {PageSize}", request.PageNumber, request.PageSize);
    }

    protected override void LogOperationSuccess(ListProductsCommand request, ListProductsResult result)
    {
        if (result != null && result.Items != null)
        {
            Logger.LogInformation("Products listed successfully. Retrieved {Count} products", result.Items.Count());
        }
        else
        {
            Logger.LogWarning("Products listing completed but result or items is null");
        }
    }
} 