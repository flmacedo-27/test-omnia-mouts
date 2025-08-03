using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer;

/// <summary>
/// AutoMapper profile for GetCustomerRequest mapping.
/// </summary>
public class GetCustomerRequestProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the GetCustomerRequestProfile class.
    /// </summary>
    public GetCustomerRequestProfile()
    {
        CreateMap<GetCustomerResult, GetCustomerResponse>();
    }
} 