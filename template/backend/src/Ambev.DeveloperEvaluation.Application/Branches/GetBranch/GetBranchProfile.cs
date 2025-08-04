using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

public class GetBranchProfile : Profile
{
    public GetBranchProfile()
    {
        CreateMap<Branch, GetBranchResult>();
    }
} 