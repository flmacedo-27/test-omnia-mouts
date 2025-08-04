using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

/// <summary>
/// Response for listing products with pagination.
/// </summary>
public class ListProductsResponse : PaginatedList<ProductListItem>
{
    /// <summary>
    /// Initializes a new instance of ListProductsResponse
    /// </summary>
    /// <param name="products">The list of products</param>
    /// <param name="totalCount">The total count of products</param>
    /// <param name="currentPage">The current page number</param>
    /// <param name="pageSize">The page size</param>
    public ListProductsResponse(IEnumerable<ProductListItem> products, int totalCount, int currentPage, int pageSize)
        : base(products.ToList(), totalCount, currentPage, pageSize)
    {
    }
}