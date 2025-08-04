using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.DeleteBranch;

public class DeleteBranchProfile : Profile
{
    public DeleteBranchProfile()
    {
        CreateMap<DeleteBranchRequest, DeleteBranchCommand>();
        CreateMap<DeleteBranchResult, DeleteBranchResponse>();
    }
} 