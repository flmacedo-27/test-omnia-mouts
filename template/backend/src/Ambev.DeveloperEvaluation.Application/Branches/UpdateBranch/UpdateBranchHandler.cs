using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

public class UpdateBranchHandler : BaseHandler<UpdateBranchCommand, UpdateBranchResult?, UpdateBranchCommandValidator>
{
    private readonly IBranchRepository _branchRepository;

    public UpdateBranchHandler(
        IBranchRepository branchRepository, 
        IMapper mapper,
        ILogger<UpdateBranchHandler> logger,
        UpdateBranchCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _branchRepository = branchRepository;
    }

    protected override async Task<UpdateBranchResult?> ExecuteAsync(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);
        
        branch.Update(request.Name, request.Code, request.Address);
        
        var updatedBranch = await _branchRepository.UpdateAsync(branch, cancellationToken);

        return Mapper.Map<UpdateBranchResult>(updatedBranch);
    }

    protected override void LogOperationStart(UpdateBranchCommand request)
    {
        Logger.LogInformation("Updating branch with ID: {BranchId}", request.Id);
    }

    protected override void LogOperationSuccess(UpdateBranchCommand request, UpdateBranchResult? result)
    {
        Logger.LogInformation("Branch updated successfully with ID: {BranchId}", request.Id);
    }
}