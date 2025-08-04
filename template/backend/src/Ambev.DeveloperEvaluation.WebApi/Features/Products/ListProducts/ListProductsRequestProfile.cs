using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

/// <summary>
/// AutoMapper profile for ListProductsRequest mapping.
/// </summary>
public class ListProductsRequestProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the ListProductsRequestProfile class.
    /// </summary>
    public ListProductsRequestProfile()
    {
        CreateMap<ListProductsRequest, ListProductsCommand>();
    }
} 