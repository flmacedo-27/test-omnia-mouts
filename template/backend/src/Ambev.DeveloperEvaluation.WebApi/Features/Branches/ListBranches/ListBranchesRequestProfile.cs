using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.ListBranches;

/// <summary>
/// AutoMapper profile for ListBranchesRequest mapping.
/// </summary>
public class ListBranchesRequestProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the ListBranchesRequestProfile class.
    /// </summary>
    public ListBranchesRequestProfile()
    {
        CreateMap<ListBranchesRequest, ListBranchesCommand>();
    }
} 