using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

public class GetCustomerProfile : Profile
{
    public GetCustomerProfile()
    {
        CreateMap<Customer, GetCustomerResult>();
    }
} 