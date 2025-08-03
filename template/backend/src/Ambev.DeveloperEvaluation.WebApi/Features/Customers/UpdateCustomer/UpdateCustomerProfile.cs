using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;

public class UpdateCustomerProfile : Profile
{
    public UpdateCustomerProfile()
    {
        CreateMap<UpdateCustomerRequest, UpdateCustomerCommand>();
        CreateMap<UpdateCustomerResult, UpdateCustomerResponse>();
    }
} 