using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

public class DeleteBranchHandler : BaseHandler<DeleteBranchCommand, DeleteBranchResult, DeleteBranchCommandValidator>
{
    private readonly IBranchRepository _branchRepository;

    public DeleteBranchHandler(
        IBranchRepository branchRepository,
        IMapper mapper,
        ILogger<DeleteBranchHandler> logger,
        DeleteBranchCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _branchRepository = branchRepository;
    }

    protected override async Task<DeleteBranchResult> ExecuteAsync(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        await _branchRepository.DeleteAsync(request.Id, cancellationToken);

        return new DeleteBranchResult
        {
            Success = true,
            Message = "Branch deleted successfully"
        };
    }

    protected override void LogOperationStart(DeleteBranchCommand request)
    {
        Logger.LogInformation("Deleting branch with ID: {BranchId}", request.Id);
    }

    protected override void LogOperationSuccess(DeleteBranchCommand request, DeleteBranchResult result)
    {
        Logger.LogInformation("Branch deleted successfully with ID: {BranchId}", request.Id);
    }
} 