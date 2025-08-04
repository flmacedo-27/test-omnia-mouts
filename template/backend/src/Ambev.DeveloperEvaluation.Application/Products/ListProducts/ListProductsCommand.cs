using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsCommand : IRequest<ListProductsResult>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 