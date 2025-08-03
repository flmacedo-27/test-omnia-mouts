using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;

public class ListCustomersProfile : Profile
{
    public ListCustomersProfile()
    {
        CreateMap<Customer, CustomerListItem>();
    }
} 