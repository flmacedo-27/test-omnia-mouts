using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

public class CreateBranchHandler : BaseHandler<CreateBranchCommand, CreateBranchResult, CreateBranchCommandValidator>
{
    private readonly IBranchRepository _branchRepository;

    public CreateBranchHandler(
        IBranchRepository branchRepository, 
        IMapper mapper,
        ILogger<CreateBranchHandler> logger,
        CreateBranchCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _branchRepository = branchRepository;
    }

    protected override async Task<CreateBranchResult> ExecuteAsync(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = new Branch
        {
            Name = request.Name,
            Code = request.Code,
            Address = request.Address
        };

        var createdBranch = await _branchRepository.CreateAsync(branch, cancellationToken);

        return Mapper.Map<CreateBranchResult>(createdBranch);
    }

    protected override void LogOperationStart(CreateBranchCommand request)
    {
        Logger.LogInformation("Creating branch with name: {BranchName}", request.Name);
    }

    protected override void LogOperationSuccess(CreateBranchCommand request, CreateBranchResult result)
    {
        if (result != null)
        {
            Logger.LogInformation("Branch created successfully with ID: {BranchId}", result.Id);
        }
        else
        {
            Logger.LogWarning("Branch creation completed but result is null");
        }
    }
} 