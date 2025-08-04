using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

public class GetBranchHandler : BaseHandler<GetBranchCommand, GetBranchResult?, GetBranchCommandValidator>
{
    private readonly IBranchRepository _branchRepository;

    public GetBranchHandler(
        IBranchRepository branchRepository, 
        IMapper mapper,
        ILogger<GetBranchHandler> logger,
        GetBranchCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _branchRepository = branchRepository;
    }

    protected override async Task<GetBranchResult?> ExecuteAsync(GetBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);
        
        return Mapper.Map<GetBranchResult>(branch);
    }

    protected override void LogOperationStart(GetBranchCommand request)
    {
        Logger.LogInformation("Getting branch with ID: {BranchId}", request.Id);
    }

    protected override void LogOperationSuccess(GetBranchCommand request, GetBranchResult? result)
    {
        Logger.LogInformation("Branch retrieved successfully with ID: {BranchId}", request.Id);
    }
} 