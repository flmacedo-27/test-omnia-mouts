using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

public class ListBranchesHandler : IRequestHandler<ListBranchesCommand, ListBranchesResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListBranchesHandler> _logger;

    public ListBranchesHandler(
        IBranchRepository branchRepository, 
        IMapper mapper,
        ILogger<ListBranchesHandler> logger)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ListBranchesResult> Handle(ListBranchesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listing branches with page {PageNumber} and size {PageSize}", request.PageNumber, request.PageSize);

        var (branches, totalCount) = await _branchRepository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

        var branchItems = _mapper.Map<IEnumerable<BranchListItem>>(branches);

        _logger.LogDebug("Found {BranchCount} branches out of {TotalCount} total", branchItems.Count(), totalCount);

        return new ListBranchesResult(branchItems, totalCount, request.PageNumber, request.PageSize);
    }
} 