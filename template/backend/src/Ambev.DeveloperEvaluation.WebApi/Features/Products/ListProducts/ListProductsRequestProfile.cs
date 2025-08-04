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
        CreateMap<ListProductsResult, ListProductsResponse>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Products));
        CreateMap<Application.Products.ListProducts.ProductListItem, ProductListItem>();
    }
} 