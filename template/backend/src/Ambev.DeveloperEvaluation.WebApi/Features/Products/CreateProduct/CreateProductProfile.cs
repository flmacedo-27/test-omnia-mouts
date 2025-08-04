using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

/// <summary>
/// AutoMapper profile for CreateProduct mapping.
/// </summary>
public class CreateProductProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the CreateProductProfile class.
    /// </summary>
    public CreateProductProfile()
    {
        CreateMap<CreateProductRequest, CreateProductCommand>();
        CreateMap<CreateProductResult, CreateProductResponse>();
    }
} 