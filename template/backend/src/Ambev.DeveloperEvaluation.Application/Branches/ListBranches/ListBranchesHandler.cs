using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

public class ListBranchesHandler : BaseHandler<ListBranchesCommand, ListBranchesResult, ListBranchesCommandValidator>
{
    private readonly IBranchRepository _branchRepository;

    public ListBranchesHandler(
        IBranchRepository branchRepository, 
        IMapper mapper,
        ILogger<ListBranchesHandler> logger,
        ListBranchesCommandValidator validator)
        : base(mapper, logger, validator)
    {
        _branchRepository = branchRepository;
    }

    protected override async Task<ListBranchesResult> ExecuteAsync(ListBranchesCommand request, CancellationToken cancellationToken)
    {
        var (branches, totalCount) = await _branchRepository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

        var branchItems = Mapper.Map<IEnumerable<BranchListItem>>(branches);

        return new ListBranchesResult(branchItems, totalCount, request.PageNumber, request.PageSize);
    }

    protected override void LogOperationStart(ListBranchesCommand request)
    {
        Logger.LogInformation("Listing branches with page {PageNumber} and size {PageSize}", request.PageNumber, request.PageSize);
    }

    protected override void LogOperationSuccess(ListBranchesCommand request, ListBranchesResult result)
    {
        if (result != null && result.Items != null)
        {
            Logger.LogInformation("Branches listed successfully. Retrieved {Count} branches", result.Items.Count());
        }
        else
        {
            Logger.LogWarning("Branches listing completed but result or items is null");
        }
    }
} 