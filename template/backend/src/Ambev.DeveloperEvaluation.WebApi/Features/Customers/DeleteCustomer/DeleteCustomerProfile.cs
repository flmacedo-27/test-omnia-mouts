using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.DeleteCustomer;

public class DeleteCustomerProfile : Profile
{
    public DeleteCustomerProfile()
    {
        CreateMap<DeleteCustomerRequest, DeleteCustomerCommand>();
        CreateMap<DeleteCustomerResult, DeleteCustomerResponse>();
    }
} 