using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

public class DeleteBranchCommand : IRequest<DeleteBranchResult>
{
    public Guid Id { get; set; }
}