using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

public class ListBranchesProfile : Profile
{
    public ListBranchesProfile()
    {
        CreateMap<Branch, BranchListItem>();
    }
} 