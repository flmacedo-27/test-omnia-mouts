using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

public class DeleteBranchHandler : IRequestHandler<DeleteBranchCommand, DeleteBranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<DeleteBranchHandler> _logger;

    public DeleteBranchHandler(
        IBranchRepository branchRepository,
        ILogger<DeleteBranchHandler> logger)
    {
        _branchRepository = branchRepository;
        _logger = logger;
    }

    public async Task<DeleteBranchResult> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting branch with ID: {BranchId}", request.Id);

        var success = await _branchRepository.DeleteAsync(request.Id, cancellationToken);
        
        if (!success)
        {
            _logger.LogWarning("Branch not found for deletion with ID: {BranchId}", request.Id);
            return new DeleteBranchResult
            {
                Success = false,
                Message = "Branch not found"
            };
        }

        _logger.LogInformation("Branch deleted successfully with ID: {BranchId}", request.Id);

        return new DeleteBranchResult
        {
            Success = true,
            Message = "Branch deleted successfully"
        };
    }
} 