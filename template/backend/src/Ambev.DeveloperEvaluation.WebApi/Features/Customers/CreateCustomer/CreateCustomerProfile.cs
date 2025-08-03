using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;

/// <summary>
/// AutoMapper profile for CreateCustomer mapping.
/// </summary>
public class CreateCustomerProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the CreateCustomerProfile class.
    /// </summary>
    public CreateCustomerProfile()
    {
        CreateMap<CreateCustomerRequest, CreateCustomerCommand>();
        CreateMap<CreateCustomerResult, CreateCustomerResponse>();
    }
} 