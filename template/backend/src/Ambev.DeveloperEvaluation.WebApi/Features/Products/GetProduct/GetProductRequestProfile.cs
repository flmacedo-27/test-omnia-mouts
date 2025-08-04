using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

/// <summary>
/// AutoMapper profile for GetProductRequest mapping.
/// </summary>
public class GetProductRequestProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the GetProductRequestProfile class.
    /// </summary>
    public GetProductRequestProfile()
    {
        CreateMap<GetProductResult, GetProductResponse>();
    }
} 