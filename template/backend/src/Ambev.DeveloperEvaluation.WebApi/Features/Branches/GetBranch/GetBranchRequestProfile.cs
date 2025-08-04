using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranch;

/// <summary>
/// AutoMapper profile for GetBranchRequest mapping.
/// </summary>
public class GetBranchRequestProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the GetBranchRequestProfile class.
    /// </summary>
    public GetBranchRequestProfile()
    {
        CreateMap<GetBranchResult, GetBranchResponse>();
    }
} 