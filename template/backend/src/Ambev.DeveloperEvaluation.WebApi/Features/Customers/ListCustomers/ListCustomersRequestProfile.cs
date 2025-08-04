using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.ListCustomers;

/// <summary>
/// AutoMapper profile for ListCustomersRequest mapping.
/// </summary>
public class ListCustomersRequestProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the ListCustomersRequestProfile class.
    /// </summary>
    public ListCustomersRequestProfile()
    {
        CreateMap<ListCustomersRequest, ListCustomersCommand>();
    }
} 