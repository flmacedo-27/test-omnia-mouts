using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

public class ListBranchesCommand : IRequest<ListBranchesResult>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 