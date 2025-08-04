using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsProfile : Profile
{    
    public ListProductsProfile()
    {
        CreateMap<Product, ProductListItem>();
    }
} 